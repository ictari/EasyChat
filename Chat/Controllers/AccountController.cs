using Chat.Infrastructure;
using Chat.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Chat.Controllers
{
    public class AccountController : AppController
    {

        [Route("login", Name = "login")]
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return Redirect("/"); ;
            }
            return View();
        }

        [HttpPost]
        [Route("login")]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier,Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, model.Username) 
                };


                var id = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

                Request.GetOwinContext()
                    .Authentication
                    .SignIn(id);
                return Redirect("/");
            }
            return View();
        }

        [Route("logout", Name = "logout")]
        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();
            return Redirect("/");
        }
    }
}