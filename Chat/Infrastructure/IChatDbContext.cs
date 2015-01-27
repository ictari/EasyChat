using System;
using System.Data.Entity;
using System.Threading.Tasks;
namespace Chat.Infrastructure
{
    public interface IChatDbContext
    {
        DbSet<Message> Messages { get; set; }
        int SaveChanges();
        
        Task<int> SaveChangesAsync();
    }
}
