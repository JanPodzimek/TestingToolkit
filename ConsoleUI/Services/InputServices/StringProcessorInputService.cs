using ConsoleUI.Interfaces;
using Spectre.Console;
using StringProcessor;
using ConsoleUI.Enums;
using ConsoleUI.Helpers;
using Serilog;

namespace ConsoleUI.Services.InputServices
{
    public class StringProcessorInputService : IInputService
    {
        private readonly IMenuService<StringProcessorMenuOption> _stringProcessorMenuService;

        public StringProcessorInputService(IMenuService<StringProcessorMenuOption> stringProcessorMenuService)
        {
            _stringProcessorMenuService = stringProcessorMenuService;
        }

        private const string Delimiter = "Add comma delimiter";
        private const string SingleQuotes = "Add single quotes ('text')";
        private const string DoubleQuotes = "Add double quotes (\"text\")";
        private const string RemoveDuplicates = "Remove duplicates (case insensitive -> \"apple\" and \"Apple\" are considered as duplication)";

        public Task Run(HttpClient httpClient)
        {
            AnsiConsole.Clear();

            var selectedMenuOption = _stringProcessorMenuService.ShowMenu();

            if (selectedMenuOption == StringProcessorMenuOption.StringMutator)
            {
                RunMutator();
            }
            else if (selectedMenuOption == StringProcessorMenuOption.StringGenerator)
            {
                RunGenerator();
            }
            else if (selectedMenuOption == StringProcessorMenuOption.Return)
            {
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }

        public List<string> GetValuesForMutation()
        {
            AnsiConsole.MarkupLine($"[yellow]Insert values. \nAfter inserting, press {Markup.Escape("[Enter]")} twice to finish:[/]");
            List<string> lines = new();
            string? line;

            while (string.IsNullOrWhiteSpace(line = Console.ReadLine()) == false)
            {
                lines.Add(line);
            }

            string userInput = string.Join(" ", lines);

            return userInput.Split(" ").ToList();
        }

        public List<string> AskUserToSelectActions()
        {
            var selectedActions = AnsiConsole.Prompt(new MultiSelectionPrompt<string>()
                .Title("\n[yellow]Select actions to apply on inserted items:[/]")
                .AddChoices(
                    Delimiter,
                    SingleQuotes,
                    DoubleQuotes,
                    RemoveDuplicates
            ));

            if (selectedActions.Contains(SingleQuotes) && selectedActions.Contains(DoubleQuotes))
            {
                throw new Exception("Cannot choose wrapping with both single and double quotes at a time.");
            }

            return selectedActions;
        }

        public List<string> ApplyActions(List<string> actions, List<string> userInput)
        {
            if (actions.Contains(RemoveDuplicates))
            {
                userInput = Mutator.RemoveDuplicates(userInput);
            }

            if (actions.Contains(SingleQuotes))
            {
                userInput = Mutator.ApplySingleQuotes(userInput);
            }
            else if (actions.Contains(DoubleQuotes))
            {
                userInput = Mutator.ApplyDoubleQuotes(userInput);
            }
            
            if (actions.Contains(Delimiter))
            {
                userInput = Mutator.ApplyDelimiter(userInput);
            }

            return userInput;
        }

        void RunMutator()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            List<string> selectedActions = new();
            List<string> userInput = GetValuesForMutation();
            bool validActions;

            do
            {
                validActions = true;

                try
                {
                    selectedActions = AskUserToSelectActions();
                }
                catch (Exception ex)
                {
                    AnsiConsole.Clear();
                    AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
                    validActions = false;
                }
            } while (validActions == false);

            userInput = ApplyActions(selectedActions, userInput);

            AnsiConsole.MarkupLine("[yellow]Output:[/]");
            foreach (var item in userInput)
            {
                Console.WriteLine($"{item}");
            }

            InputHelper.End();
        }

        void RunGenerator()
        {
            int length = 0;

            string userInput = InputHelper.GetUserInput("[yellow]How long string to generate? (chars): [/] ", "[red]Input cannot be empty. Please try again.[/]");
            bool charCounterDeactivated = int.TryParse(userInput, out length);

            if (charCounterDeactivated)
            {
                length = int.Parse(userInput);
                AnsiConsole.MarkupLine("[yellow]\nOutput:[/]");
                Console.WriteLine(Generator.GenerateString(length));
            } 
            else
            {
                Console.WriteLine();
                Log.Logger.Information($"Length: {userInput.Length}");
                Log.Logger.Warning($"Char counter was automatically applied on invalid user's input.");
            }

            InputHelper.End();
        }
    }
}
