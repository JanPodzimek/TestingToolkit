using ConsoleUI.Interfaces;
using ConsoleUI.Services;
using ConsoleUI.Enums;
using ConsoleUI.Services.InputServices;
using ConsoleUI.Services.MenuServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Net.Http.Headers;
using TradeApiCaller.RegistrationNumber;

namespace ConsoleUI
{
    internal class Program
    {
        static Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Application starting...");

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json", false, true)
                       .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true)
                       .AddJsonFile("credentials.json", optional: true, reloadOnChange: true)
                       .AddEnvironmentVariables();
                })

                .UseSerilog((context, logConfig) =>
                    logConfig.ReadFrom.Configuration(context.Configuration)
                      .Enrich.FromLogContext()
                      .WriteTo.Console())

                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<StringProcessorInputService>();
                    services.AddTransient<UserProcessorInputService>();
                    services.AddTransient<RegistrationNumberInputService>();
                    services.AddTransient<HashIdInputService>();

                    services.AddHttpClient<WorkflowService>((client) =>
                    {
                        client.BaseAddress = new Uri(context.Configuration["Api:BaseUrl"]);
                        client.DefaultRequestHeaders.UserAgent.ParseAdd("Chrome/135.0.0.0");
                        client.DefaultRequestHeaders.Accept
                              .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    });

                    services.AddSingleton<IMenuService<MainMenuOption>, MainMenuService>();
                    services.AddSingleton<IMenuService<StringProcessorMenuOption>, StringProcessorMenuService>();
                    services.AddSingleton<IMenuService<UserProcessorMenuOption>, UserProcessorMenuService>();
                    services.AddSingleton<IMenuService<RegistrationNumberType>, RegistrationNumberMenuService>();

                    services.AddSingleton<IInputServiceFactory, InputServiceFactory>();
                })
                .Build();

            var config = host.Services.GetRequiredService<IConfiguration>();
            var credentialsSection = config.GetSection("AdminCredentials");
            if (!credentialsSection.Exists())
            {
                Console.WriteLine();
                Log.Logger.Error("Missing file 'credentials.json'");
                Log.Logger.Error("Create the file in app root folder and fill it with valid admin credentials");
                Log.Logger.Error("Check ReadME.md for more information");
                Console.WriteLine();
                Log.Logger.Information("Press any key to close the app...");
                Console.ReadKey();
                Environment.Exit(1);
            }

            var svc = host.Services.GetRequiredService<WorkflowService>();
            return svc.Run();
        }
    }
}
