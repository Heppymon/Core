using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyBotCore.Jobs;
using MyBotCore.Services;
using MyBotCore.Shared.Interfaces.Services;
using MyBotCore.Shared.Settings;
using MyBotDb;
using Npgsql;
using Quartz;
using System.Reflection;

namespace MyBotCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<BotSettings>(Configuration.GetSection("BotSettings"));
            services.AddScoped<ITgBotService, TgBotService>();

            ConfigureSwagger(services);
            // ConfigureQuartz(services);
            ConfigureDatabase(services);

            services.AddControllers();
        }



        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("./v1/swagger.json", "TGbotManager v1"));
            //app.UseHttpsRedirection();
            //app.UseHsts();
            app.UseStaticFiles();
            app.UseRouting();
            // app.UseMigration();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // endpoints.MapHealthChecks("/health");
                // endpoints.MapMetrics("/metrics");
            });
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
            {
                var dbConfig = Configuration.GetSection("DbSettings").Get<DbSettings>();
                var builder = new NpgsqlConnectionStringBuilder
                {
                    Host = "", //herokuDbConf.Host,
                    Port = 5432, //herokuDbConf.Port,
                    Username = "", //herokuDbConf.Username,
                    Password = "", //herokuDbConf.Password,
                    Database = "", //herokuDbConf.Database,
                    SslMode = SslMode.Disable, //herokuDbConf.SslMode,
                    TrustServerCertificate = true, //herokuDbConf.TrustServerCertificate
                };
                cs = builder.ToString();
            }

            services.AddDbContext<MyBotContext>(options => options.UseNpgsql(cs));
        }
    }
}
