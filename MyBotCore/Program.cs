using MyBotCore.Shared.Settings;
using Serilog;
using Serilog.Configuration;

namespace MyBotCore
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";
            //Read Configuration from appSettings
            var config = new ConfigurationBuilder()
                // load default config
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                // then load current config
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                // load env variables from docker-compose config
                .AddEnvironmentVariables()
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

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            LoggerSettings loggerSettings = null;
            var host = Host.CreateDefaultBuilder(args)
            .UseDefaultServiceProvider((context, options) =>
            {
                options.ValidateOnBuild = false;
                options.ValidateScopes = false;
            })
            .ConfigureAppConfiguration((context, builder) =>
            {
                var builtConfig = builder.Build();
                loggerSettings = builtConfig.GetSection(nameof(LoggerSettings)).Get<LoggerSettings>();

            })
            // .UseSerilog() TODO: Add serilog into
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                .UseStartup<Startup>()
                .UseWebRoot("wwwroot");
            });
            return host;
        }


        private static void InitializeLogger(IConfigurationRoot config)
        {
            var loggerSettings = config.GetSection(nameof(LoggerSettings)).Get<LoggerSettings>();
        }
    }
}