using Telegram.Bot.Types.ReplyMarkups;

namespace MyBotCore.Services.Keyboard
{
    public class KeyboardBase
    {
        private KeyboardBase() { }
        //private static KeyboardBase singleton;
        public static KeyboardBase Create(MenuData data, KeyboardSettings settings)
        {
            // if(singleton != null)
            // return singleton;

            var newMenu = new KeyboardBase()
            {
                Name = data.Name,
                Description = data.Description,
                RootName = data.RootName,
                Keys = data.Keys,
                KeyboardSettings = settings
            };

            if (newMenu.Keys.Count > 0)
            {
                var inlineKb = new List<InlineKeyboardButton[]>();
                foreach (var key in newMenu.Keys)
                    inlineKb.Add(new[] { InlineKeyboardButton.WithCallbackData(text: key.Name, callbackData: key.ShortName) });

                newMenu.InlineMenu = new InlineKeyboardMarkup(inlineKb.ToArray());
                newMenu.ReplyMenu = KeyboardBase.CreateReplyMenu(newMenu.Keys, settings);
            }
            //singleton = newMenu;
            return newMenu; //singleton;
        }

        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string RootName { get; protected set; }
        public List<MenuKey> Keys { get; protected set; }
        public InlineKeyboardMarkup InlineMenu { get; protected set; }
        public ReplyKeyboardMarkup ReplyMenu { get; protected set; }
        public KeyboardSettings KeyboardSettings { get; protected set; }

        private static ReplyKeyboardMarkup CreateReplyMenu(List<MenuKey> keys, KeyboardSettings settings)
        {
            if (keys.Count == 0)
                throw new Exception("Keys array can`t be null or empty!");

            // Делим кнопки на группы (размер группы определяется в настройках клавиатуры)
            
            var arrayRowsCount = (int)Math.Ceiling((decimal)keys.Count / settings.ReplyKeyboardKeysInRow);

            var keybs = new KeyboardButton[arrayRowsCount][];

            for (int i = 0; i < arrayRowsCount; i++)
            {
                var rowList = new List<KeyboardButton>();

                for (int j = 0; j < settings.ReplyKeyboardKeysInRow; j++)
                {
                    var itemIndex = j + (i * settings.ReplyKeyboardKeysInRow);
                    var currentKey = keys.ElementAtOrDefault(itemIndex);
                    if (currentKey is not null)
                        rowList.Add(new KeyboardButton(currentKey.Name));
                }
                keybs[i] = rowList.ToArray();
            }

            return new ReplyKeyboardMarkup(keybs) { ResizeKeyboard = true };
        }
    }
}
