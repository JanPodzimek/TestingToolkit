using ConsoleUI.Enums;
using Microsoft.Extensions.Options;

namespace ConsoleUI.Services.MenuServices
{
    public class RegistrationNumberMenuService : IMenuService<RegistrationNumberMenuOption>
    {
        public RegistrationNumberMenuOption ShowMenu()
        {
            var options = new Dictionary<string, RegistrationNumberMenuOption>()
            {
                { "Vat payer (standard Trade registration)", RegistrationNumberMenuOption.VatPayer },
                { "Vat payer with more bank accounts", RegistrationNumberMenuOption.VatPayerWithMultipleBankAccounts },
                { "Vat payer without bank account", RegistrationNumberMenuOption.VatPayerWithoutBankAccount },
                { "No vat payer", RegistrationNumberMenuOption.NoVatPayer },
                { "Without address", RegistrationNumberMenuOption.WithoutAddress },
                { "Return", RegistrationNumberMenuOption.Return }
            };

            return MenuHelper.ShowMenu("\n[yellow]Choose type of registration number:[/]", options);
        }
    }
}
