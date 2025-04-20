using ConsoleUI.Interfaces;
using ConsoleUI.Services.MenuServices;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using ConsoleUI.Enums;

namespace ConsoleUI.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IConfiguration _config;
        private readonly IInputService _stringProcessorInputService;
        private readonly IMenuService<MenuOption> _menuService;


        public WorkflowService(
            IConfiguration config,
            IInputService inputService,
            IMenuService<MenuOption> menuService)
        {
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
                var selectedMenuOption = _menuService.ShowMenu();

                if (selectedMenuOption == MenuOption.StringProcessor)
                {
                    _stringProcessorInputService.Run();
                }
                else if (selectedMenuOption == MenuOption.CreateUser)
                {

                }
                else if (selectedMenuOption == MenuOption.Exit)
                {
                    Console.WriteLine($"{_config.GetValue<string>("Greeting")}");
                    return;
                }
            }
        }
    }
}
