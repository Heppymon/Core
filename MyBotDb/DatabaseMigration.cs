using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MyBotDb
{
    public static class DatabaseMigrate
    {
        public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetService<MyBotContext>();
            context.Database.EnsureCreated();
            if (context is not null)
                context.Database.Migrate();

            return app;
        }
    }
}
