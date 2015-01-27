using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Chat.Infrastructure
{
    public class ChatDbContext: DbContext, IChatDbContext
    {
        public DbSet<Message> Messages { get; set; }
    }
}