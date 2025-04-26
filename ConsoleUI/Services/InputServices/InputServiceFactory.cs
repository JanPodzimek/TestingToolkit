using ConsoleUI.Enums;
using ConsoleUI.Interfaces;

namespace ConsoleUI.Services.InputServices
{
    public class InputServiceFactory(
            StringProcessorInputService stringSvc,
            UserProcessorInputService userSvc,
            RegistrationNumberInputService regNumberSvc,
            HashIdInputService hashIdSvc) : IInputServiceFactory
    {
        public IInputService Get(MainMenuOption option) =>
            option switch
            {
                MainMenuOption.StringProcessor => stringSvc,
                MainMenuOption.CreateUser => userSvc,
                MainMenuOption.GetRegistrationNumber => regNumberSvc,
                MainMenuOption.ResolveHashId => hashIdSvc,
                _ => throw new ArgumentOutOfRangeException(nameof(option))
            };
    }
}
