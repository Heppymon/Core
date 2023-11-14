namespace MyBotCore.Services.Keyboard
{
    public class MenuData
    {
        public MenuData(string name, string rootName, List<MenuKey> keys, string description = "")
        {
            Name = name;
            RootName = rootName;
            Keys = keys;
            Description = description;
        }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string RootName { get; protected set; }
        public List<MenuKey> Keys { get; }
        public List<MenuKey> MenuKeys { get; protected set; }
    }
}
