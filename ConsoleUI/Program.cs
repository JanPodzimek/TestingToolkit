using ConsoleUI.Configuration;
using ConsoleUI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new GlobalSetup();
            var workflowService = GlobalSetup.ServiceProvider.GetRequiredService<WorkflowService>();
            workflowService.Run();
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettins.json", optional: false, reloadOnChange: true);
        }
    }
}
