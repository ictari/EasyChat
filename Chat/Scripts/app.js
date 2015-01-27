function Model() {
    var self = this;
    self.username = '';
    self.messages = ko.observableArray();
    self.message = ko.observable();
    self.onEnter = function (data, e) {
        if (e.keyCode == 13 && self.canSend())
            self.sendMessage();
        return true;
    }

    self.sendMessage = function () {
        chat.server.sendMessage(self.message());
        self.message('');
    }

    self.canSend = ko.computed(function () {
        var message = self.message() || '';
        return !_.isEmpty(message.trim());
    });

    self.users = ko.observableArray();
    self.initalized = ko.observable(true);

    self.removeUser = function (id) {
        self.users.remove(function (user) {
            return user.id == id;
        });
    }

    self.addUser = function (id, username) {
        var notExist = !_.findWhere(self.users(), { id: id });
        if (notExist)
           self.users.push({ id: id, username: username, typing: ko.observable(false) });
    }

    self.logout = function () {
        $('#logout').submit();
    }

    self.error = ko.observable(false).extend({ rateLimit: 1000 });
}
var model = new Model();

ko.applyBindings(model);

var modal = $('#loading').modal({ keyboard: false, backdrop: 'static' });
var messageEl = $('.messages');

function scrollToBottom() {
    messageEl.scrollTop(messageEl.prop("scrollHeight"));
}

var chat = $.connection.chatHub;

chat.client.sendMessage = function (username, message) {
    var el = messageEl[0];
    var isBottom = el.offsetHeight + el.scrollTop >= el.scrollHeight;
    model.messages.push({ username: username, message: message });
    if (isBottom)
        scrollToBottom();
};

chat.client.userConnected = model.addUser;

chat.client.userDisconnected = model.removeUser;

chat.client.typing = function (id, typing) {
    var user = _.findWhere(model.users(), { id: id });
    if (user)
        user.typing(typing);
};

model.canSend.subscribe(function () {
    chat.server.typing(model.canSend());
});
$.connection.hub.start().done(function () {
    $.getJSON('/initialize').done(function (data) {
        _.each(data.users, function (user) {
            model.users.push({ id: user.id, username: user.username, typing: ko.observable(user.typing) });
        });
        _.each(data.messages, function (message) {
            model.messages.unshift({ username: message.username, message: message.message });
        });
        scrollToBottom();
        model.initalized(true);
        modal.modal('hide');
    });
});

window.onbeforeunload = function (e) {
    $.connection.hub.stop();
};