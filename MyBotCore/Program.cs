using MyBotCore.Shared.Settings;
using Serilog;
using Serilog.Configuration;

namespace MyBotCore
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //Read Configuration from appSettings
            var config = new ConfigurationBuilder()
                // load default config
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                // then load current config
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .Build();

            InitializeLogger(config);
            try
            {
                Log.Information($"TgBot starting on {environment} environment.");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                .UseStartup<Startup>()
                .UseWebRoot("wwwroot");
            });

        private static void InitializeLogger(IConfigurationRoot config)
        {
            var loggerSettings = config.GetSection(nameof(LoggerSettings)).Get<LoggerSettings>();
        }
    }
}