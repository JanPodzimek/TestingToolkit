using Spectre.Console;

namespace ConsoleUI.Helpers
{
    public static class InputHelper
    {
        public static void End()
        {
            Console.WriteLine();
            Console.WriteLine("Press any to key to continue...");
            Console.ReadKey(true);
        }

        public static string GetUserInput(string description, string warning)
        {
            string? textInput;

            do
            {
                AnsiConsole.Markup($"{description}");
                textInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(textInput))
                {
                    AnsiConsole.Clear();
                    AnsiConsole.MarkupLine($"{warning}");
                }

            } while (string.IsNullOrEmpty(textInput));

            return textInput;
        }
    }
}
