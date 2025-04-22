using ConsoleUI.Interfaces;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using ConsoleUI.Enums;

namespace ConsoleUI.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IConfiguration _config;
        private readonly IInputServiceFactory _factory;
        private readonly IMenuService<MainMenuOption> _menuService;

        public WorkflowService(
            IConfiguration config,
            IInputServiceFactory factory,
            IMenuService<MainMenuOption> menuService)
        {
            _config = config;
            _factory = factory;
            _menuService = menuService;
        }

        public async Task Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                AnsiConsole.Clear();
                var selected = _menuService.ShowMenu();

                if (selected == MainMenuOption.StringProcessor)
                {
                    // get the StringProcessorInputService from the factory
                    var svc = _factory.Get(MainMenuOption.StringProcessor);
                    await svc.Run();
                }
                else if (selected == MainMenuOption.CreateUser)
                {
                    // get the UserProcessorInputService from the factory
                    var svc = _factory.Get(MainMenuOption.CreateUser);
                    await svc.Run();
                }
                else if (selected == MainMenuOption.Exit)
                {
                    Console.WriteLine(_config.GetValue<string>("Greeting"));
                    return;
                }
            }
        }
    }
}
