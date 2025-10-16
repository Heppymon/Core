using MyBotCore.Shared.Settings;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

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

            InitializeLogger(config, environment);
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
            .UseSerilog() // Uses Serilog instead of default .NET Logger
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                .UseStartup<Startup>()
                .UseWebRoot("wwwroot");
            });
            return host;
        }

        private static void InitializeLogger(IConfigurationRoot config, string environment)
        {
            var loggerSettings = config.GetSection(nameof(LoggerSettings)).Get<LoggerSettings>();
            var loggerConfiguration = new LoggerConfiguration();
            loggerConfiguration.WriteTo.Console();

            if (loggerSettings != null && !string.IsNullOrEmpty(loggerSettings.SentryDSN))
            {
                loggerConfiguration.WriteTo.Sentry(o =>
                {
                    o.Debug = environment == Environments.Development;

                    // Debug and higher are stored as breadcrumbs (default is Information)
                    o.MinimumBreadcrumbLevel = o.Debug ? LogEventLevel.Debug : LogEventLevel.Information;
                    // Warning and higher is sent as event (default is Error)
                    o.MinimumEventLevel = (LogEventLevel)loggerSettings.LogEventLevel;

                    // Set TracesSampleRate to 1.0 to capture 100% of transactions for performance monitoring.
                    // We recommend adjusting this value in production.
                    o.TracesSampleRate = 1.0;
                    o.Dsn = loggerSettings.SentryDSN;
                    o.AttachStacktrace = true;
                    // send PII like the username of the user logged in to the device
                    o.SendDefaultPii = true;
                });
            }

            if (loggerSettings is null || string.IsNullOrEmpty(loggerSettings.FilePath))
            {
                // uncomment if file logging is required
                Log.Logger = loggerConfiguration.CreateLogger();
                Log.Information("LOG TO FILE DISABLED");
                return;
            }

            if (loggerSettings.DisableEFInformationLogs)
                loggerConfiguration.MinimumLevel.Override("Microsoft", (LogEventLevel)loggerSettings.LogEventLevel);

            loggerConfiguration.Enrich.WithExceptionDetails();
            loggerConfiguration.Enrich.WithEnvironmentName();
            // loggerConfiguration.Enrich.WithMachineName();
            if (!string.IsNullOrEmpty(loggerSettings.ApplicationName))
                loggerConfiguration.Enrich.WithProperty("ApplicationName", loggerSettings.ApplicationName);

            var logBasePath = Environment.GetEnvironmentVariable("APP_LOG_PATH");
            if (string.IsNullOrEmpty(logBasePath))
                logBasePath = loggerSettings.FilePath;
            var logFilePath = Path.Combine(logBasePath, $"{loggerSettings.ApplicationName}-{environment}", ".log");
            Log.Information("Log path: {LogPath}", logFilePath);

            loggerConfiguration.WriteTo.File(logFilePath,
                rollingInterval: Enum.Parse<RollingInterval>(loggerSettings.RollingInterval, true),
                fileSizeLimitBytes: loggerSettings.FileSizeLimitBytes,
                retainedFileCountLimit: loggerSettings.RetainedFileLimit,
                outputTemplate: loggerSettings.Template);

            Log.Logger = loggerConfiguration.CreateLogger();
        }
    }
}

