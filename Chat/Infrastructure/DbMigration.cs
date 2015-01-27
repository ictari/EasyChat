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
                new Message{ MessageText="Hi everybody", Username="Yakup"},
                new Message{ MessageText="Hi mann!!", Username="James Hetfield"},
                new Message{ MessageText="Hey can you play Master Of Puppets for me", Username="Yakup"},
                new Message{ MessageText="Sure listen :) <a href='https://www.youtube.com/watch?v=kV-2Q8QtCY4'>Master Of Puppets</a>", Username="James Hetfield"},
                new Message{ MessageText="Hey this is new one, i haven't seen it before", Username="Yakup"},
                new Message{ MessageText="Oh yeahhhh", Username="James Hetfield"},
                new Message{ MessageText="Rock On :)", Username="Yakup"},
            };

            context.Messages.AddRange(list);
        }
    }
}