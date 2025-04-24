using ConsoleUI.Enums;
using ConsoleUI.Interfaces;

namespace ConsoleUI.Services.InputServices
{
    public class InputServiceFactory : IInputServiceFactory
    {
        private readonly StringProcessorInputService _stringSvc;
        private readonly UserProcessorInputService _userSvc;
        private readonly RegistrationNumberInputService _regNumberSvc;

        public InputServiceFactory(
            StringProcessorInputService stringSvc, 
            UserProcessorInputService userSvc, 
            RegistrationNumberInputService regNumberSvc)
        {
            _stringSvc = stringSvc;
            _userSvc = userSvc;
            _regNumberSvc = regNumberSvc;
        }

        public IInputService Get(MainMenuOption option) =>
            option switch
            {
                MainMenuOption.StringProcessor => _stringSvc,
                MainMenuOption.CreateUser => _userSvc,
                MainMenuOption.GetRegistrationNumber => _regNumberSvc,
                _ => throw new ArgumentOutOfRangeException(nameof(option))
            };
    }
}
