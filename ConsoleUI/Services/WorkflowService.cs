using ConsoleUI.InputServices;
using Spectre.Console;

namespace ConsoleUI.Services
{
    public class WorkflowService
    {
        private readonly StringProcessorInputService _stringProcessorInputService;
        private readonly MenuService _menuService;
        

        public WorkflowService(StringProcessorInputService inputService, MenuService menuService)
        {
            _stringProcessorInputService = inputService;
            _menuService = menuService;
        }

        public void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                AnsiConsole.Clear();
                var selectedMenuOption = _menuService.ShowMainMenu();

                if (selectedMenuOption == MenuService.MenuOption.StringProcessor)
                {
                    _stringProcessorInputService.Run();
                }
                else if (selectedMenuOption == MenuService.MenuOption.CreateUser)
                {

                }
                else if (selectedMenuOption == MenuService.MenuOption.Exit)
                {
                    Console.WriteLine("👋 Goodbye!");
                    return;
                }
            }
        }
    }
}
