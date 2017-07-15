using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Infrastructure
{
    public class Message: Entity
    {
        public string Username { get; set; }

        public string Text { get; set; }

        public DateTimeOffset Date { get; set; }

    }
}
