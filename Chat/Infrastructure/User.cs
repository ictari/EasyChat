using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Infrastructure
{
    public class User
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public bool Typing { get; set; }

        public HashSet<string> Connections { get; set; }

        public User()
        {
            this.Connections = new HashSet<string>();
        }
    }
}