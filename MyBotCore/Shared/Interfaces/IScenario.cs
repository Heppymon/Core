using Telegram.Bot.Types;

namespace MyBotCore.Shared.Interfaces
{
    public interface IScenario
    {
        public Task OnUnknown(Update update);
        public Task OnMessageReceived(Message message);
        public Task OnEditedMessageReceived(Message message);
        public Task OnChannelPost(Message message);
        public Task OnEditedChannelPost(Message message);
        public Task OnCallbackQuery(CallbackQuery query);
        public Task OnInlineQuery(InlineQuery query);
        public Task OnChosenInlineResult(ChosenInlineResult inlineResult);
    }
}
