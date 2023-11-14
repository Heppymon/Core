using MyBotCore.Services.Keyboard;
using MyBotCore.Shared.Interfaces;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MyBotCore.Services
{
    public class MainScenario : IScenario
    {
        private readonly ITelegramBotClient botClient;
        private Keyboards keyboards;

        public MainScenario(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            keyboards = Keyboards.Create();
        }

        public async Task OnMessageReceived(Message message)
        {
            Log.Information("Receive message type: {MessageType}", message.Type);
            if (message.Type != MessageType.Text)
                return;

            if (message.Text!.ToLower() == "/start")
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Привет!",
                    replyMarkup: keyboards.RootMenu.ReplyMenu);
                return;
            }

            await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                text: "",
                replyMarkup: keyboards.RootMenu.ReplyMenu);
            return;
        }

        public async Task OnCallbackQuery(CallbackQuery query)
        {
            throw new NotImplementedException();
        }

        public Task OnChannelPost(Message message)
        {
            throw new NotImplementedException();
        }

        public Task OnChosenInlineResult(ChosenInlineResult inlineResult)
        {
            throw new NotImplementedException();
        }

        public Task OnEditedChannelPost(Message message)
        {
            throw new NotImplementedException();
        }

        public Task OnEditedMessageReceived(Message message)
        {
            throw new NotImplementedException();
        }

        public Task OnInlineQuery(InlineQuery query)
        {
            throw new NotImplementedException();
        }

        public Task OnUnknown(Update update)
        {
            throw new NotImplementedException();
        }
    }
}
