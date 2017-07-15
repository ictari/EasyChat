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
                new Message{ Text="Hi everybody!", Username="Yakup"},
                new Message{ Text="Hi!!", Username="James" },
                new Message{ Text="Hi!", Username="John" },
            };

            context.Messages.AddRange(list);
        }
    }
}