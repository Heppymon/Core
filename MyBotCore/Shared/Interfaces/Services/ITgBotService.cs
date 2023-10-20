using Telegram.Bot.Types;

namespace MyBotCore.Shared.Interfaces.Services
{
    public interface ITgBotService
    {
        /// <summary>
        /// Main service method, choose an action for telegram update event data
        /// </summary>
        /// <param name="update">Telegram actionUpdate entity with incomming data</param>
        public Task EchoAsync(Update update);
    }
}
