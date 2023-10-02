using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyBotCore.Jobs;
using MyBotCore.Middleware;
using MyBotCore.Services;
using MyBotCore.Services.Hosted;
using MyBotCore.Shared.Settings;
using MyBotDb;
using Quartz;
using System.Reflection;
using Telegram.Bot;

namespace MyBotCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Step one - configure core settings of app
            ConfigureSettings(services);
            // Step two - configure hosted services
            ConfigureAllServices(services);

            // Step three - all others
            ConfigureSwagger(services);
            // ConfigureQuartz(services);
            ConfigureDatabase(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            // Add exception middleware, 1st row!
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("./v1/swagger.json", "TGbotManager v1"));
            //app.UseHttpsRedirection();
            //app.UseHsts();
            app.UseMigration();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // endpoints.MapHealthChecks("/health");
                // endpoints.MapMetrics("/metrics");
            });
        }

        private void ConfigureSettings(IServiceCollection services)
        {
            services.Configure<BotSettings>(Configuration.GetSection("BotSettings"));
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<QuartzSettings>(Configuration.GetSection("QuartzSettings"));
            services.Configure<OpenAiSettings>(Configuration.GetSection("OpenAiSettings"));
        }

        private void ConfigureAllServices(IServiceCollection services)
        {
            // uncomment this for supress request model validation
            // services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            services.AddControllers().AddNewtonsoftJson();

            services.AddControllers();
            var botConfig = Configuration.GetSection("BotSettings").Get<BotSettings>();
            services.AddHttpClient("tgwebhook")
                .AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(botConfig.BotToken, httpClient));

            services.AddHostedService<ConfigureWebhook>();
            services.AddScoped<TgBotService>();
        }

        private void ConfigureQuartz(IServiceCollection services)
        {
            var quartzConfig = Configuration.GetSection("QuartzSettings").Get<QuartzSettings>();
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                var jobKey = new JobKey(quartzConfig.JobName);
                q.AddJob<Sheduler>(op => op.WithIdentity(jobKey));
                q.AddTrigger(opts => opts.ForJob(jobKey).WithIdentity(quartzConfig.TriggerName)
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(quartzConfig.CoolDown).RepeatForever()));
            });
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyTgBot", Version = "v1" });
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.ApiKey,
                        In = ParameterLocation.Header,
                        Scheme = "Bearer",
                        Name = "Authorization",
                        Description = "Format:'{Bearer}'{space}''{Value-JWT}'"
                    });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme{Reference = new OpenApiReference{Type = ReferenceType.SecurityScheme,Id = "Bearer"},
                            Scheme = "oauth2",Name = "Bearer",In = ParameterLocation.Header,},new List<string>()
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            var cs = Configuration.GetConnectionString("TgbotDatabase");

            if (string.IsNullOrWhiteSpace(cs))
                cs = Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>().PostgresDb;

            services.AddDbContext<MyBotContext>(options => options.UseNpgsql(cs));
        }
    }
}
