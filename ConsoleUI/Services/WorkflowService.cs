using ConsoleUI.Interfaces;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using ConsoleUI.Enums;
using TradeApiCaller.Authentication;
using Serilog;

namespace ConsoleUI.Services
{
    public class WorkflowService(
            IConfiguration config,
            IInputServiceFactory factory,
            IMenuService<MainMenuOption> menuService,
            HttpClient httpClient) : IWorkflowService
    {
        public async Task Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Log.Logger.Information("Retrieving authorization token for API calls...");

            var username = config.GetValue<string>("Api:AdminCredentials:Login");
            var password = config.GetValue<string>("Api:AdminCredentials:Password");

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException("Admin credentials are missing in appsettings.");
            }

            var token = await IdentityService.GetTokenValueAsync(username, password);
            httpClient.DefaultRequestHeaders.Add("authorization", $"Bearer {token}");

            while (true)
            {
                AnsiConsole.Clear();
                var selected = menuService.ShowMenu();

                if (selected == MainMenuOption.StringProcessor)
                {
                    // get the StringProcessorInputService from the factory
                    var svc = factory.Get(MainMenuOption.StringProcessor);
                    await svc.Run(httpClient: null);
                }
                else if (selected == MainMenuOption.CreateUser)
                {
                    // get the UserProcessorInputService from the factory
                    var svc = factory.Get(MainMenuOption.CreateUser);
                    await svc.Run(httpClient: null);
                }
                else if (selected == MainMenuOption.GetRegistrationNumber)
                {
                    var svc = factory.Get(MainMenuOption.GetRegistrationNumber);
                    await svc.Run(httpClient);
                }
                else if (selected == MainMenuOption.Exit)
                {
                    Console.WriteLine(config.GetValue<string>("Greeting"));
                    return;
                }
                
            }
        }
    }
}
