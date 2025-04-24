using ConsoleUI.Enums;

namespace ConsoleUI.Services.MenuServices
{
    public class UserProcessorMenuService : IMenuService<UserProcessorMenuOption>
    {
        public UserProcessorMenuOption ShowMenu()
        {
            var options = new Dictionary<string, UserProcessorMenuOption>
            {
                { "✅ Generate random login", UserProcessorMenuOption.RandomLogin },
                { "✅ Set custom login", UserProcessorMenuOption.CustomLogin },
                { "🔙 Return", UserProcessorMenuOption.Return }
            };

            return MenuHelper.ShowMenu("\n[yellow]Choose feature:[/]", options);
        }
    }
}
