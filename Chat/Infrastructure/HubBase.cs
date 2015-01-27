using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Infrastructure
{

    /// <summary>
    /// CustomHub<T> required for providing settable Clients property for mocking
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HubBase<T> : Hub<T> where T : class
    {
        private IHubCallerConnectionContext<T> clients;
        public new IHubCallerConnectionContext<T> Clients
        {
            get
            {
                return clients ?? base.Clients;
            }
            set
            {
                clients = value;
            }
        }
    }
}