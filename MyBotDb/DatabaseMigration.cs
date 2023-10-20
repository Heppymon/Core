using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace MyBotDb
{
    public static class DatabaseMigrate
    {
        public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
        {
            try
            {
                Log.Information("Executing Database.Migrate()...");

                using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var ctx = serviceScope.ServiceProvider.GetService<MainBotContext>();
                ctx?.Database.EnsureCreated();
                ctx?.Database.Migrate();

                // EnsureInitialResources().ConfigureAwait(false).GetAwaiter().GetResult();
                Log.Information("Executing Database.Migrate() finished");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Executing Database.Migrate() failed");
                throw;
            }

            return app;
        }
    }
}
