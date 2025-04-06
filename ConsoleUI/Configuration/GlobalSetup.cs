using ConsoleUI.Services;
using Microsoft.Extensions.DependencyInjection;
using ToolkitBE;

namespace ConsoleUI.Configuration
{
    public class GlobalSetup
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public GlobalSetup()
        {
            var services = new ServiceCollection();
            services.AddSingleton<DataProcessor>();
            services.AddSingleton<InputService>();
            services.AddSingleton<WorkflowService>();
            services.AddSingleton<MenuService>();
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
