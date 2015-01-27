using Chat.Hubs;
using Chat.Infrastructure;
using Microsoft.AspNet.SignalR.Hubs;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Tests
{
    public partial class HubTests
    {
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
        public class TestableChatHub : ChatHub
        {
            public Mock<HubCallerContext> MockContext { get; set; }

            public Mock<IHubCallerConnectionContext<IChatHubClient>> MockClients { get; set; }

            public Mock<IChatHubClient> MockAll { get; set; }

            public Mock<IChatHubClient> MockOthers { get; set; }

            public Mock<IChatHubClient> MockCaller { get; set; }

            public TestableChatHub(Mock<IConnectionManager> manager, Mock<IChatDbContext> chatDbContext)
                : base(manager.Object, chatDbContext.Object)
            {
                MockContext = new Mock<HubCallerContext>();
                MockContext.SetupGet(m => m.User).Returns(GetUser());

                Context = MockContext.Object;

                MockClients = new Mock<IHubCallerConnectionContext<IChatHubClient>>();
                
                Clients = MockClients.Object;

                MockAll = new Mock<IChatHubClient>();
                MockOthers = new Mock<IChatHubClient>();
                MockCaller = new Mock<IChatHubClient>();
                MockClients.Setup(m => m.All).Returns(MockAll.Object);
                MockClients.Setup(m => m.Others).Returns(MockOthers.Object);
                MockClients.Setup(m => m.Caller).Returns(MockCaller.Object);
            }

            public IPrincipal GetUser()
            {
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier,Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, "Alex") 
                };

                var user = new Mock<IPrincipal>();
                user.SetupGet(p => p.Identity).Returns(new ClaimsIdentity(claims));

                return user.Object;
            }
        }
    }
}
