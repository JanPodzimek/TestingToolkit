using ConsoleUI.Interfaces;
using ConsoleUI.Services;
using ConsoleUI.Services.InputServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ConsoleUI
{
    public static class Startup
    {
        public static void Run(string[] args)
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
                    services.AddSingleton<IMenuService, MenuService>();
                    services.AddSingleton<IInputService, StringProcessorInputService>();
                    services.AddSingleton<IWorkflowService, WorkflowService>();
                })
                .UseSerilog()
                .Build();

            var svc = ActivatorUtilities.CreateInstance<WorkflowService>(host.Services);
            svc.Run();
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
