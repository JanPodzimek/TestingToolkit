using System.Net;
using ConsoleUI.Enums;
using ConsoleUI.Helpers;
using ConsoleUI.Interfaces;
using Serilog;
using Spectre.Console;
using UserProcessor;

namespace ConsoleUI.Services.InputServices
{
    public class UserProcessorInputService : IInputService
    {
        private readonly IMenuService<UserProcessorMenuOption> _userProcessorMenuService;

        public UserProcessorInputService(IMenuService<UserProcessorMenuOption> userProcessorMenuService)
        {
            _userProcessorMenuService = userProcessorMenuService;
        }

        public Task Run(HttpClient client)
        {
            AnsiConsole.Clear();

            var selectedMenuOption = _userProcessorMenuService.ShowMenu();

            if (selectedMenuOption == UserProcessorMenuOption.RandomLogin)
            {
                return GenerateRandomLogin();
            }
            else if (selectedMenuOption == UserProcessorMenuOption.CustomLogin)
            {
                return GenerateCustomLogin();
            }
            else if (selectedMenuOption == UserProcessorMenuOption.Return)
            {
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }

        public async Task GenerateCustomLogin()
        {
            string customLogin = InputHelper.GetUserInput(
                "[yellow]Enter the login: [/] ",
                "[red]Input cannot be empty. Please try again.[/]"
                );

            var result = await GenerateLogin(customLogin);

            if (result.ResponseCode == HttpStatusCode.OK && string.IsNullOrEmpty(result.Message))
            {
                Log.Logger.Information("Response status code: {responseCodeInt} {responseCodeText}",
                        (int)result.ResponseCode,
                        result.ResponseCode);
                Log.Logger.Information("User successfully created!");
                Console.WriteLine();

                Log.Logger.Information($"Login:    {result.Login}");
                Log.Logger.Information($"Password: {result.Password}");
            }
            else if (result.ResponseCode == HttpStatusCode.OK && !string.IsNullOrEmpty(result.Message))
            {
                int i = result.Message.IndexOf(':');
                string alteredMessage = i >= 0 ? result.Message.Substring(i + 1).Trim() : result.Message;

                Console.WriteLine();
                Log.Logger.Error($"Invalid login: \"{customLogin}\"");
                Log.Logger.Error($"Response message: \"{alteredMessage}\"");
                Console.WriteLine();
                Log.Logger.Information("Try again");
            }
            else
            {
                Log.Logger.Warning("Response status code: {responseCodeInt} {responseCodeText}",
                        (int)result.ResponseCode,
                        result.ResponseCode);
            }

            InputHelper.End();
        }

        public async Task GenerateRandomLogin()
        {
            var result = await GenerateLogin();

            if (result.ResponseCode == HttpStatusCode.OK && string.IsNullOrEmpty(result.Message))
            {
                Log.Logger.Information("Response status code: {responseCodeInt} {responseCodeText}",
                        (int)result.ResponseCode,
                        result.ResponseCode);
                Log.Logger.Information("User successfully created!");
                Console.WriteLine();

                Log.Logger.Information($"Login:    {result.Login}");
                Log.Logger.Information($"Password: {result.Password}");
            }
            else
            {
                Log.Logger.Warning("Response status code: {responseCodeInt} {responseCodeText}",
                    (int)result.ResponseCode,
                    result.ResponseCode);
                Console.WriteLine();
            }

            InputHelper.End();
        }

        public async Task<RegistrationResultModel> GenerateLogin(string? login = default)
        {
            RegistrationResultModel result = new();
            AnsiConsole.Clear();

            try
            {
                Log.Logger.Information("Processing request...");
                result = await Creator.CreateUser(login);
            }
            catch (Exception ex)
            {
                throw new($"Error while creating new user:[/] {ex.Message}");
            }

            return result;
        }
    }
}
