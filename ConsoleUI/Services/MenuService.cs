
using Spectre.Console;

namespace ConsoleUI.Services
{
    public class MenuService
    {
        public enum MenuOption
        {
            InsertValues,
            Exit
        }

        private const string InsertValuesText = "✅ Insert values";
        private const string ExitText = "⚠️ Exit";

        public MenuOption ShowMainMenu()
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[yellow]Choose an action:[/]")
                    .HighlightStyle("green")
                    .AddChoices(new[]
                    {
                    InsertValuesText,
                    ExitText
                    }));

            return choice switch
            {
                InsertValuesText => MenuOption.InsertValues,
                ExitText => MenuOption.Exit,
                _ => throw new Exception("Unrecognized menu option")
            };
        }
    }
}
