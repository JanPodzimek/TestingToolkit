using ConsoleUI.Interfaces;
using Spectre.Console;
using StringProcessor;
using ConsoleUI.Enums;

namespace ConsoleUI.Services.InputServices
{
    public class StringProcessorInputService : IInputService
    {
        private readonly IMenuService<StringProcessorMenuOption> _stringProcessorMenuService;

        public StringProcessorInputService(
            IMenuService<StringProcessorMenuOption> stringProcessorMenuService)
        {
            _stringProcessorMenuService = stringProcessorMenuService;
        }

        private const string Delimiter = "Add comma delimiter";
        private const string SingleQuotes = "Add single quotes ('text')";
        private const string DoubleQuotes = "Add double quotes (\"text\")";
        private const string RemoveDuplicates = "Remove duplicates (case insensitive -> \"apple\" and \"Apple\" are considered as duplication)";

        public void Run()
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
                return;
            }
        }

        public void End()
        {
            Console.WriteLine();
            Console.WriteLine("Press any to key to continue...");
            Console.ReadKey(true);
        }

        public string GetStringLength()
        {
            string? textInput;

            do
            {
                AnsiConsole.Markup("[yellow]How long string? (chars): [/] ");
                textInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(textInput))
                {
                    AnsiConsole.Clear();
                    AnsiConsole.MarkupLine("[red]Input cannot be empty. Please try again.[/]");
                }

            } while (string.IsNullOrEmpty(textInput));

            return textInput;
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

            End();
        }

        void RunGenerator()
        {
            int length = 0;

            string userInput = GetStringLength();
            bool charCounterDeactivated = int.TryParse(userInput, out length);

            if (charCounterDeactivated)
            {
                length = int.Parse(userInput);
                AnsiConsole.MarkupLine("[yellow]\nOutput:[/]");
                Console.WriteLine(Generator.GenerateString(length));
            } 
            else
            {
                AnsiConsole.MarkupLine($"[red]{userInput.Length} -> Char counter was applied on invalid user's input.[/]");
            }

            End();
        }
    }
}
