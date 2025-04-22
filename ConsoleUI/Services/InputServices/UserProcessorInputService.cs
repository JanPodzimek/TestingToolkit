using System.Net;
using System.Text;
using ConsoleUI.Helpers;
using ConsoleUI.Interfaces;
using Serilog;
using Spectre.Console;
using UserProcessor;

namespace ConsoleUI.Services.InputServices
{
    public class UserProcessorInputService : IInputService
    {
        public async Task Run()
        {
            AnsiConsole.Clear();

            try
            {
                Log.Logger.Information("Processing request...");
                var result = await Creator.CreateUser();

                if (result.ResponseCode == HttpStatusCode.OK)
                {
                    Log.Logger.Information("Response status code: {responseCodeInt} {responseCodeText}", 
                        (int)result.ResponseCode, 
                        result.ResponseCode);
                    Console.WriteLine();
                    Log.Logger.Information($"Login   : {result.Login}");
                    Log.Logger.Information($"Password: {result.Password}");
                }
                else
                {
                    Log.Logger.Warning("Response status code: {responseCodeInt} {responseCodeText}",
                        (int)result.ResponseCode,
                        result.ResponseCode);
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error creating user:[/] {ex.Message}");
                InputHelper.End();
                
                return;
            }

            InputHelper.End();
        }
    }
}
