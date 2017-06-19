using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Chat.Infrastructure
{
    public class MigrationConfiguration : DbMigrationsConfiguration<ChatDbContext>
    {
        public MigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ChatDbContext context)
        {
            if (context.Messages.Any())
                return;


            var list = new List<Message> {
                new Message{ MessageText="Hello everybody!", Username="John", DateUtc = DateTime.UtcNow },
                new Message{ MessageText="Hi!", Username="James", DateUtc = DateTime.UtcNow },
            };

            context.Messages.AddRange(list);
        }
    }
}