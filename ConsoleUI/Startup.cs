using ConsoleUI.Interfaces;
using ConsoleUI.Services;
using ConsoleUI.Enums;
using ConsoleUI.Services.InputServices;
using ConsoleUI.Services.MenuServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ConsoleUI
{
    public static class Startup
    {
        public static Task Run(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Application starting...");

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<StringProcessorInputService>();
                    services.AddTransient<UserProcessorInputService>();

                    services.AddSingleton<IMenuService<MainMenuOption>, MainMenuService>();
                    services.AddSingleton<IMenuService<StringProcessorMenuOption>, StringProcessorMenuService>();

                    services.AddSingleton<IInputServiceFactory, InputServiceFactory>();
                })
                .UseSerilog()
                .Build();

            var svc = ActivatorUtilities.CreateInstance<WorkflowService>(host.Services);
            return svc.Run();
        }

        private static void BuildConfig(IConfigurationBuilder builder)
        {
            builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }
    }
}
