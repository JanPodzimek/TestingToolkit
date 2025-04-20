using ConsoleUI.Enums;

namespace ConsoleUI.Services.MenuServices

{
    public class MenuService : IMenuService<MenuOption>
    {
        public MenuOption ShowMenu()
        {
            var options = new Dictionary<string, MenuOption>
            {
                { "✅ String Processor", MenuOption.StringProcessor },
                { "✅ Create new user (IDS)", MenuOption.CreateUser },
                { "⚠️ Exit", MenuOption.Exit }
            };

            return MenuHelper.ShowMenu("\n[yellow]Choose tool:[/]", options);
        }
    }
}

