using ConsoleUI.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace ConsoleUI.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly ILogger<WorkflowService> _log;
        private readonly IConfiguration _config;
        private readonly IInputService _stringProcessorInputService;
        private readonly IMenuService _menuService;


        public WorkflowService(
            ILogger<WorkflowService> log,
            IConfiguration config,
            IInputService inputService,
            IMenuService menuService)
        {
            _log = log;
            _config = config;
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
                    Console.WriteLine($"{_config.GetValue<string>("Greeting")}");
                    return;
                }
            }
        }
    }
}
