using Telegram.Bot.Types.ReplyMarkups;

namespace MyBotCore.Services.Keyboard
{
    public class KeyboardBase
    {
        public KeyboardBase(string name) 
        { 
        
        
        }
        public string Name { get; protected set; }
        public string ShortName { get; protected set; }
        public string Description { get; protected set; }

        public List<MenuKey> TgKeys { get; protected set; }
        public InlineKeyboardMarkup InlineMenu { get; protected set; }
        public ReplyKeyboardMarkup ReplyMenu { get; protected set; }
    }
}
