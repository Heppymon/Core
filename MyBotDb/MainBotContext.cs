using Microsoft.EntityFrameworkCore;
using MyBotDb.Models;

namespace MyBotDb
{
    public class MainBotContext : DbContext
    {
        public MainBotContext() : base() { }
        public MainBotContext(DbContextOptions options) : base(options) { }
        public DbSet<DbUser> Users { get; set; }
        public DbSet<DbEventData> EventsData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
