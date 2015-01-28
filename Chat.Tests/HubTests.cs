using Chat.Hubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Infrastructure;
using Microsoft.AspNet.SignalR.Hubs;
using Moq;
using System.Data.Entity;
using System.Security.Principal;
using System.Security.Claims;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.Identity;

namespace Chat.Tests
{
    [TestClass]
    public class HubTests
    {
        [TestMethod]
        public async Task ShouldSendMessageAndSaveToDatabase()
        {
            var mockChatContext = GetMockChatDbContext();
            var mockConnectionManager = new Mock<IConnectionManager>();
            var hub = new TestableChatHub(mockConnectionManager, mockChatContext);

            var message = "Lets chat";
            var username = "Alex";

            hub.MockAll.Setup(m => m.SendMessage(username, message)).Verifiable();

            await hub.SendMessage(message);

            hub.MockAll.Verify(m => m.SendMessage(username, message), Times.Once);
            mockChatContext.Verify(s => s.SaveChangesAsync(), Times.Once);

            var savedMessage = mockChatContext.Object.Messages.First();
            Assert.AreEqual(savedMessage.Username, username);
            Assert.AreEqual(savedMessage.MessageText, message);

            hub.MockAll.VerifyAll();
        }

        [TestMethod]
        public void ShouldSendTypingInformation()
        {
            var mockChatContext = GetMockChatDbContext();
            var mockConnectionManager = new Mock<IConnectionManager>();
            var hub = new TestableChatHub(mockConnectionManager, mockChatContext);

            var id = Guid.NewGuid().ToString();
            var typing = true;
            var mockAll = hub.MockAll;

            mockAll.Setup(m => m.Typing(id, typing)).Verifiable();

            mockAll.Object.Typing(id, typing);

            mockAll.Verify(m => m.Typing(id, typing), Times.Once);
            mockAll.VerifyAll();
        }

        [TestMethod]
        public async Task ShouldConnectSuccessfullyOnFirstRequest()
        {
            var mockChatContext = GetMockChatDbContext();
            var mockConnectionManager = new Mock<IConnectionManager>();
            var hub = new TestableChatHub(mockConnectionManager, mockChatContext);

            var userid = hub.Identity.GetUserId();
            var name = hub.Identity.Name;
            var connectionId = Guid.NewGuid().ToString();
            hub.MockContext.SetupGet(m => m.ConnectionId).Returns(connectionId);
            hub.MockOthers.Setup(m => m.UserConnected(userid, name)).Verifiable();

            mockConnectionManager.Setup(m => m.Add(userid, connectionId, name)).Returns(true).Verifiable();

            await hub.OnConnected();

            mockConnectionManager.Verify();
            hub.MockOthers.Verify();
        }

        [TestMethod]
        public async Task ShouldRemoveConnectionOnDisconnect()
        {
            var mockChatContext = GetMockChatDbContext();
            var mockConnectionManager = new Mock<IConnectionManager>();
            var hub = new TestableChatHub(mockConnectionManager, mockChatContext);

            var userid = hub.Identity.GetUserId();
            var connectionId = Guid.NewGuid().ToString();
            hub.MockContext.SetupGet(m => m.ConnectionId).Returns(connectionId);

            await hub.OnDisconnected(true);

            mockConnectionManager.Verify(m => m.Remove(userid, connectionId), Times.Once);

        }

        public Mock<IChatDbContext> GetMockChatDbContext()
        {
            // Create some test data
            var data = new List<Message> { };

            // Create a mock set and context
            var set = new Mock<DbSet<Message>>().SetupData(data);

            var context = new Mock<IChatDbContext>();
            context.Setup(c => c.Messages).Returns(set.Object);
            return context;
        }
    }


}
