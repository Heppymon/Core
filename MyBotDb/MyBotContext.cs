using Microsoft.EntityFrameworkCore;
using MyBotDb.Models;

namespace MyBotDb
{
    public class MyBotContext : DbContext
    {
        // Накинуть тут таблиц
        public DbSet<DbUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
