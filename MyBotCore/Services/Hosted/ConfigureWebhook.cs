using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Microsoft.Extensions.Options;
using MyBotCore.Shared.Settings;
using Serilog;

namespace MyBotCore.Services.Hosted
{
    public class ConfigureWebhook : IHostedService
    {
        private readonly IServiceProvider services;
        private readonly BotSettings botSettings;

        public ConfigureWebhook(IServiceProvider serviceProvider,
                                IOptions<BotSettings> botSettings)
        {
            services = serviceProvider;
            this.botSettings = botSettings.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = services.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
            var webhookAddress = @$"{botSettings.HostAddress}/api/Tg/Hook";

            Log.Information("Setting webhook: {WebhookAddress}", webhookAddress);
            await botClient.SetWebhookAsync(
                url: webhookAddress,
                allowedUpdates: Array.Empty<UpdateType>(),
                cancellationToken: cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = services.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            // Remove webhook upon app shutdown
            // Log.Information("Removing webhook");
            await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}
