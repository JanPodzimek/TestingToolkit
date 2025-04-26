using ConsoleUI.Helpers;
using ConsoleUI.Interfaces;
using Serilog;
using TradeApiCaller.HashId;

namespace ConsoleUI.Services.InputServices
{
    public class HashIdInputService : IInputService
    {
        public async Task Run(HttpClient client)
        {
            string userInput = InputHelper.GetUserInput(
                "[yellow]Enter either Id (int) or HashId (string): [/] ", 
                "[red]Input cannot be empty. Please try again.[/]");

            string result = await HashIdProcessor.ProcessHashId(client, userInput);
            string separator = new string('=', result.Length);
            
            Console.WriteLine();
            Log.Logger.Information(separator);
            Log.Logger.Information(result);
            Log.Logger.Information(separator);

            InputHelper.End();
        }
    }
}
