using System;
using Spectre.Console;
using ToolkitBE;
using ToolkitBE.Extensions;

namespace ConsoleUI.Services
{
    public class InputService
    {
        private readonly DataProcessor _dataProcessor;
        public InputService(DataProcessor dataProcessor)
        {
            _dataProcessor = dataProcessor;
        }

        private const string Delimiter = "Add comma delimiter";
        private const string SingleQuotes = "Add single quotes ('text')";
        private const string DoubleQuotes = "Add double quotes (\"text\")";

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

            return userInput.CreateList();
        }

        public List<string> AskUserToSelectActions()
        {
            var selectedActions = AnsiConsole.Prompt(new MultiSelectionPrompt<string>()
                .Title("\n[yellow]Select actions to apply on inserted items:[/]")
                .AddChoices(
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
