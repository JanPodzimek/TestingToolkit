using ConsoleUI.Enums;

namespace ConsoleUI.Services.MenuServices

{
    public class MainMenuService : IMenuService<MainMenuOption>
    {
        public MainMenuOption ShowMenu()
        {
            var options = new Dictionary<string, MainMenuOption>
            {
                { "⚙️ String Processor", MainMenuOption.StringProcessor },
                { "🧑 Create new user (IDS)", MainMenuOption.CreateUser },
                { "🆔 Get registration number (ICO)", MainMenuOption.GetRegistrationNumber },
                { "⚠️ Exit", MainMenuOption.Exit }
            };

            return MenuHelper.ShowMenu("\n[yellow]Choose tool:[/]", options);
        }
    }
}

