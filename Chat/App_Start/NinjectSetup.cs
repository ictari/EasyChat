using Microsoft.Owin;
using Ninject;
using Owin;
using Ninject.Web.Common.OwinHost;
using System.Reflection;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Chat.Infrastructure;
using Chat.App_Start;
using Ninject.Web.Common;
using System;
using ConnectionManager = Chat.Infrastructure.ConnectionManager;
using IConnectionManager = Chat.Infrastructure.IConnectionManager;
namespace Chat
{
    public partial class Startup
    {
        public void ConfigureNinject(IAppBuilder app)
        {
            var kernel = CreateKernel();
            SignalRSetup(app, kernel);
            app.UseNinjectMiddleware(() =>
            {               
                return kernel;
            });
        }

        private void SignalRSetup(IAppBuilder app, StandardKernel kernel)
        {
            var config = new HubConfiguration { Resolver = new NinjectSignalRDependencyResolver(kernel) };

            // Make long polling connections wait a maximum of 110 seconds for a
            // response. When that time expires, trigger a timeout command and
            // make the client reconnect.
            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(10);

            // Wait a maximum of 30 seconds after a transport connection is lost
            // before raising the Disconnected event to terminate the SignalR connection.
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(10);

            // For transports other than long polling, send a keepalive packet every
            // 10 seconds. 
            // This value must be no more than 1/3 of the DisconnectTimeout value.
            GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(3);

            GlobalHost.DependencyResolver = config.Resolver;
            app.MapSignalR(config);
        }

        private StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            SetupBindings(kernel);
            return kernel;
        }

        private void SetupBindings(IKernel kernel)
        {
            kernel.Bind<IConnectionManager>().To<ConnectionManager>().InSingletonScope();
            kernel.Bind<IChatDbContext>().To<ChatDbContext>().InRequestScope();
        }

    }
}
