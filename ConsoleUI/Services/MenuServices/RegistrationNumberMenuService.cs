using ConsoleUI.Enums;
using TradeApiCaller.RegistrationNumber;

namespace ConsoleUI.Services.MenuServices
{
    public class RegistrationNumberMenuService : IMenuService<RegistrationNumberType>
    {
        public RegistrationNumberType ShowMenu()
        {
            var options = new Dictionary<string, RegistrationNumberType>()
            {
                { "Vat payer (standard Trade registration)", RegistrationNumberType.VatPayer },
                { "Vat payer with more bank accounts", RegistrationNumberType.VatPayerWithMultipleBankAccounts },
                { "Vat payer without bank account", RegistrationNumberType.VatPayerWithoutBankAccount },
                { "No vat payer", RegistrationNumberType.NoVatPayer },
                { "Without address in ARES", RegistrationNumberType.WithoutAddress },
                { "Return", RegistrationNumberType.Return }
            };

            return MenuHelper.ShowMenu("\n[yellow]Choose type of registration number:[/]", options);
        }
    }
}
