using MyBotCore.Shared.Const;
using MyBotCore.Shared.Enums;
using MyBotCore.Shared.Exceptions;
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
        private readonly IScenario scenario;
        public TgBotService(ITelegramBotClient botClient, IScenario scenario) // Сюда нужно положить сценарий, для действий бота
        {
            this.botClient = botClient;
            this.scenario = scenario;
        }

        public async Task EchoAsync(Update update)
        {
            if (update is null)
                throw new BusinessLogicException(ApiErrorCode.NullUpdateModel);

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
            catch (Exception exception)
            {
                await HandleErrorAsync(exception);
            }
        }

        public async Task<string> GetSomeString()
        {
            return "rdy";
        }

        private async Task BotOnMessageReceived(Message message)
        {
            // logger.LogInformation("Receive message type: {MessageType}", message.Type);
            if (message.Type != MessageType.Text)
                return;

            var command = message.Text!.ToLower().Split(' ')[0];

            if (command == "/start")
            {
                const string usage = "Используйте кнопки меню для работы с ботом, если их не видно, " +
                    "нажмите на квадрат с точками в правом нижнем углу экрана :)";

                //return await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: usage /*replyMarkup: keyboardsService.MainMenu.ReplyKeyboard*/);
            }
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
