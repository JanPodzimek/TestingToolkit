using Spectre.Console;

namespace ConsoleUI.Services
{
    public class WorkflowService
    {
        private readonly InputService _inputService;
        private readonly MenuService _menuService;

        public WorkflowService(InputService inputService, MenuService menuService)
        {
            _inputService = inputService;
            _menuService = menuService;
        }

        public void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            while (true)
            {
                AnsiConsole.Clear();
                var selectedMenuOption = _menuService.ShowMainMenu();

                if (selectedMenuOption == MenuService.MenuOption.Exit)
                {
                    Console.WriteLine("👋 Goodbye!");
                    return;
                }

                List<string> selectedActions = new();
                List<string> userInput = _inputService.InputConsumer();
                bool validActions;

                do
                {
                    validActions = true;

                    try
                    {
                        selectedActions = _inputService.AskUserToSelectActions();
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.Clear();
                        AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
                        validActions = false;
                    }
                } while (validActions == false);

                userInput = _inputService.ApplyActions(selectedActions, userInput);

                AnsiConsole.MarkupLine("[yellow]Output:[/]");
                foreach (var item in userInput)
                {
                    Console.WriteLine($"{item}");
                }

                End();
            }
        }

        public void End()
        {
            Console.WriteLine();
            Console.WriteLine("Press any to key to continue...");
            Console.ReadLine();
        }
    }
}
