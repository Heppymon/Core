using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Microsoft.Extensions.Options;
using MyBotCore.Shared.Settings;
using Serilog;

namespace MyBotCore.Services.Hosted
{
    public class ConfigureWebhook : IHostedService
    {
        private readonly IServiceProvider _services;
        private readonly BotSettings botSettings;

        public ConfigureWebhook(IServiceProvider serviceProvider,
                                IOptions<BotSettings> botSettings)
        {
            _services = serviceProvider;
            this.botSettings = botSettings.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            // Configure custom endpoint per Telegram API recommendations:
            // https://core.telegram.org/bots/api#setwebhook
            // If you'd like to make sure that the Webhook request comes from Telegram, we recommend
            // using a secret path in the URL, e.g. https://www.example.com/<token>.
            // Since nobody else knows your bot's token, you can be pretty sure it's us.
            // var webhookAddress = @$"{_botConfig.HostAddress}/bot/{_botConfig.BotToken}";

            var webhookAddress = @$"{botSettings.HostAddress}/Tg/HookPost";

            // Log.Information("Setting webhook: {WebhookAddress}", webhookAddress);
            await botClient.SetWebhookAsync(
                url: webhookAddress,
                allowedUpdates: Array.Empty<UpdateType>(),
                cancellationToken: cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            // Remove webhook upon app shutdown
            // Log.Information("Removing webhook");
            await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}
