using System.Net.Http;
using ConsoleUI.Enums;
using ConsoleUI.Interfaces;

namespace ConsoleUI.Services.InputServices
{
    public class RegistrationNumberInputService : IInputService
    {
        private readonly IMenuService<RegistrationNumberMenuOption> _registrationNumberMenuService;

        public RegistrationNumberInputService(IMenuService<RegistrationNumberMenuOption> registrationNumberMenuService, HttpClient httpClient)
        {
            _registrationNumberMenuService = registrationNumberMenuService;
        }

        public async Task Run(HttpClient httpClient)
        {
            var selectedMenuOption = _registrationNumberMenuService.ShowMenu();

            if (selectedMenuOption == RegistrationNumberMenuOption.VatPayer)
            {
                var response = await httpClient.GetAsync("/users");
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
            }
            else if (selectedMenuOption == RegistrationNumberMenuOption.VatPayerWithMultipleBankAccounts)
            {

            }
            else if (selectedMenuOption == RegistrationNumberMenuOption.VatPayerWithoutBankAccount)
            {

            }
            else if (selectedMenuOption == RegistrationNumberMenuOption.NoVatPayer)
            {

            }
            else if (selectedMenuOption == RegistrationNumberMenuOption.WithoutAddress)
            {

            }
            else if (selectedMenuOption == RegistrationNumberMenuOption.Return)
            {

            }

            return;
        }
    }
}
