using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Chat.Infrastructure;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using System.Security.Principal;
using Microsoft.AspNet.SignalR.Hubs;

namespace Chat.Hubs
{
    [Authorize]
    public class ChatHub : HubBase<IChatHubClient>
    {
        private readonly IConnectionManager connectionManager;
        private readonly IChatDbContext chatContext;
        private readonly int maximumUserLimit = 20;
        public IIdentity Identity { get { return Context.User.Identity; } }

        public ChatHub(IConnectionManager connectionManager, IChatDbContext chatContext)
        {
            this.connectionManager = connectionManager;
            this.chatContext = chatContext;
        }

        [UserLimitAuthorize]
        public async Task SendMessage(string message)
        {
            Clients.All.SendMessage(Identity.Name, message);

            var messageEntity = new Message
            {
                Username = Identity.Name,
                MessageText = message,
                Date = DateTimeOffset.UtcNow
            };

            chatContext.Messages.Add(messageEntity);
            await chatContext.SaveChangesAsync();

        }

        [UserLimitAuthorize]
        public void Typing(bool typing)
        {
            var user = connectionManager.GetUser(Identity.GetUserId());
            user.Typing = typing;

            Clients.All.Typing(Identity.GetUserId(), typing);
        }

        public override Task OnConnected()
        {
            Connected();
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            Connected();

            return base.OnReconnected();
        }

        private void Connected()
        {
            var key = Identity.GetUserId();
            if (UserLimitNotReached() || connectionManager.Exist(key)) // Double-checked locking http://en.wikipedia.org/wiki/Double-checked_locking
            {
                lock (connectionManager)
                {
                    if (UserLimitNotReached() || connectionManager.Exist(key))
                    {
                        var newUser = connectionManager.Add(key, Context.ConnectionId, Identity.Name);

                        if (newUser)
                            Clients.Others.UserConnected(key, Identity.Name);

                        return;
                    }
                }
            }

            Clients.Caller.LimitReached();
        }

        private bool UserLimitNotReached()
        {
            return connectionManager.KeyCount < maximumUserLimit;
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var userRemoved = connectionManager.Remove(Identity.GetUserId(), Context.ConnectionId);

            if (userRemoved)
                Clients.All.UserDisconnected(Identity.GetUserId());

            return base.OnDisconnected(stopCalled);
        }

    }
}