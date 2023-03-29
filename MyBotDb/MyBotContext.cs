using Microsoft.EntityFrameworkCore;

namespace MyBotDb
{
    public class MyBotContext : DbContext
    {
        // Накинуть тут таблиц


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
