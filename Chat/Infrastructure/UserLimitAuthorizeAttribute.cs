using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Chat.Infrastructure
{
    public class UserLimitAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool UserAuthorized(IPrincipal user)
        {
            if (user == null)
                return false;

            var manager = GlobalHost.DependencyResolver.Resolve<IConnectionManager>();

            //if user is not in list then it is already rejected because of maximum user limititation
            return manager.Exist(user.Identity.GetUserId());
        }
    }
}