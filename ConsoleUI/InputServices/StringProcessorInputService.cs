using ConsoleUI.Interfaces;
using Spectre.Console;
using ToolkitBE;
using ToolkitBE.Extensions;

namespace ConsoleUI.StringProcessorTool
{
    public class StringProcessorInputService : IInputService
    {
        private readonly StringProcessorService _dataProcessor;
        public StringProcessorInputService(StringProcessorService dataProcessor)
        {
            _dataProcessor = dataProcessor;
        }

        private const string Delimiter = "Add comma delimiter";
        private const string SingleQuotes = "Add single quotes ('text')";
        private const string DoubleQuotes = "Add double quotes (\"text\")";
        private const string RemoveDuplicates = "Remove duplicates (case insensitive -> \"apple\" and \"Apple\" are considered as duplication)";

        public void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            List<string> selectedActions = new();
            List<string> userInput = InputConsumer();
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

        public void End()
        {
            Console.WriteLine();
            Console.WriteLine("Press any to key to continue...");
            Console.ReadLine();
        }

        public List<string> InputConsumer()
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
                    RemoveDuplicates,
                    Delimiter,
                    SingleQuotes,
                    DoubleQuotes
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
                userInput = _dataProcessor.RemoveDuplicates(userInput);
            }

            if (actions.Contains(SingleQuotes))
            {
                userInput = _dataProcessor.ApplySingleQuotes(userInput);
            }
            else if (actions.Contains(DoubleQuotes))
            {
                userInput = _dataProcessor.ApplyDoubleQuotes(userInput);
            }
            
            if (actions.Contains(Delimiter))
            {
                userInput = _dataProcessor.ApplyDelimiter(userInput);
            }

            return userInput;
        }
    }
}
