using Chat.Models;
using System.Collections.Concurrent;
using System.Web.Mvc;
using Microsoft.Owin.Host.SystemWeb;
using System.Web.Security;
using System.Linq;
using Chat.Hubs;
using Chat.Infrastructure;
using System.Collections.Generic;
using System.Web;

namespace Chat.Controllers
{ 
    [Authorize]
    public class ChatController : AppController
    {
        private readonly IChatDbContext chatContext;
        private readonly IConnectionManager connectionManager;

        public ChatController(IChatDbContext chatContext, IConnectionManager connectionManager)
        {
            this.chatContext = chatContext;
            this.connectionManager = connectionManager;
        }


        public ActionResult Index()
        {
            return View();
        }

        [Route("initialize")]
        public ActionResult GetUsers()
        {          
            var users = connectionManager.GetUsers();
            var messages = chatContext.Messages.OrderByDescending(m => m.Date)
                                      .Take(15)
                                      .Select(s => new { s.Username, Message = s.Text });
            return Json(new
            {
                Users = users,
                Messages = messages
            });
        }

    }
}