using MyBotCore.Shared.Const;
using MyBotCore.Shared.Interfaces.Services;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MyBotCore.Services
{
    public class TgBotService : ITgBotService
    {
        private ITelegramBotClient botClient;

        public TgBotService(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
        }

        public async Task EchoAsync(Update update)
        {
            var handler = update.Type switch
            {
                // UpdateType.Unknown:
                // UpdateType.ChannelPost:
                // UpdateType.EditedChannelPost:
                // UpdateType.ShippingQuery:
                // UpdateType.PreCheckoutQuery:
                // UpdateType.Poll:
                UpdateType.Message => BotOnMessageReceived(update.Message!),
                UpdateType.EditedMessage => BotOnMessageReceived(update.EditedMessage!),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(update.CallbackQuery!),
                // UpdateType.InlineQuery => BotOnInlineQueryReceived(update.InlineQuery!),
                UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(update.ChosenInlineResult!),
                _ => UnknownUpdateHandlerAsync(update)
            };

            try
            {
                await handler;
            }
#pragma warning disable CA1031
            catch (Exception exception)
#pragma warning restore CA1031
            {
                await HandleErrorAsync(exception);
            }
        }

        private async Task BotOnMessageReceived(Message message)
        {
            // logger.LogInformation("Receive message type: {MessageType}", message.Type);
            if (message.Type != MessageType.Text)
                return;
            var command = message.Text!.ToLower().Split(' ')[0];
            var action = command switch
            {
                // "марафон" => StartMarathon(message),
                // "помощь" => HelpUser(message),
                // "настройки" => Settings(message),
                // "профиль" => Profile(message),
                // "выход" => Quit(message),
                _ => Usage(message)
            };
            Message sentMessage = await action;
            // logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.MessageId);
        }

        private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery)
        {
            var commands = callbackQuery.Data!.Split(' ');
            var action = commands[0] switch
            {
                // MarathonIdCommand => GetMarathon(callbackQuery, commands[1]),
                _ => UsageHidden(callbackQuery)
            };
            try
            {
                Message sentMessage = await action;
                // logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.MessageId);
            }
            catch (Exception ex) { /* logger.LogInformation(ex.Message); */}
        }

        private Task BotOnChosenInlineResultReceived(ChosenInlineResult chosenInlineResult)
        {
            // logger.LogInformation("Received inline result: {ChosenInlineResultId}", chosenInlineResult.ResultId);
            return Task.CompletedTask;
        }

        private async Task<Message> Usage(Message message)
        {
            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: BotAnswers.Usage, replyMarkup: TgKeyboards.MainMenu);
        }

        private async Task<Message> UsageHidden(CallbackQuery callbackQuery)
        {
            return await botClient.SendTextMessageAsync(chatId: callbackQuery.Message.Chat.Id,
                text: BotAnswers.SomeGoesWrong, replyMarkup: TgKeyboards.MainMenu);
        }

        private Task UnknownUpdateHandlerAsync(Update update)
        {
            // logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
            return Task.CompletedTask;
        }

        private Task HandleErrorAsync(Exception exception)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            // logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
