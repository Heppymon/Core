using Telegram.Bot.Types.ReplyMarkups;

namespace MyBotCore.Shared.Const
{
    public class TgKeyboards
    {
        public static ReplyKeyboardMarkup MainMenu { get => BuildMainMenu(); }

        private static ReplyKeyboardMarkup BuildMainMenu()
        {
            var Keyboard = new KeyboardButton[][]
            {
                    new KeyboardButton[]
                    {
                        new KeyboardButton("Пункт 1"),
                        new KeyboardButton("Пункт 2"),
                        new KeyboardButton("Пункт 3")
                    }
            };
            return new ReplyKeyboardMarkup(Keyboard)
            {
                ResizeKeyboard = true,
                InputFieldPlaceholder = "Для работы с ботом используйте кнопки 👇"
            };
        }

    }
}
