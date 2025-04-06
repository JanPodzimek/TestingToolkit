using Spectre.Console;

namespace ConsoleUI.Services
{
    public class MenuService
    {
        public enum MenuOption
        {
            StringProcessor,
            CreateUser,
            Exit
        }

        private const string StringProcessorText = "✅ String processor";
        private const string CreateUserText = "✅ Create new user (IDS)";
        private const string ExitText = "⚠️ Exit";

        public MenuOption ShowMainMenu()
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[yellow]Choose a tool:[/]")
                    .HighlightStyle("green")
                    .AddChoices(new[]
                    {
                    StringProcessorText,
                    CreateUserText,
                    ExitText
                    }));

            return choice switch
            {
                StringProcessorText => MenuOption.StringProcessor,
                CreateUserText => MenuOption.CreateUser,
                ExitText => MenuOption.Exit,
                _ => throw new Exception("Unrecognized menu option")
            };
        }
    }
}
