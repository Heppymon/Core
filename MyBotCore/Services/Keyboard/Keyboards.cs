namespace MyBotCore.Services.Keyboard
{
    public class Keyboards
    {
        public KeyboardBase RootMenu { get; private set; }

        private Keyboards()
        {
            var rootMenuKeys = new List<MenuKey>()
            {
                new MenuKey() { Name = "Info", ShortName = "Info", Description = "Bot information"},
                new MenuKey() { Name = "Test1", ShortName = "Test", Description = "Test"},
                new MenuKey() { Name = "Test2", ShortName = "Test", Description = "Test"},
                new MenuKey() { Name = "Info", ShortName = "Info", Description = "Bot information"},
            };
            RootMenu = KeyboardBase.Create(new MenuData("Root", "", rootMenuKeys), new KeyboardSettings() { ReplyKeyboardKeysInRow = 2});
        }

        private static Keyboards? singleton;
        public static Keyboards Create()
        {
            singleton ??= new Keyboards();
            return singleton;
        }
    }
}
