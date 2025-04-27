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

            var login = config.GetValue<string>("AdminCredentials:Login");
            var password = config.GetValue<string>("AdminCredentials:Password");
            string token = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                {
                    throw new InvalidOperationException("Admin credentials are missing in 'credentials.json'");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Log.Logger.Error(ex.Message);
                Log.Logger.Error("Add valid admin credentials and run the app again");
                return;
            }

            try
            {
                token = await IdentityService.GetTokenValueAsync(login, password);
            }
            catch (Exception)
            {
                return;
            }

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
                else if (selected == MainMenuOption.ResolveHashId)
                {
                    var svc = factory.Get(MainMenuOption.ResolveHashId);
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
