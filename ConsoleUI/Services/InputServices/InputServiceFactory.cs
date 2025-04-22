using ConsoleUI.Enums;
using ConsoleUI.Interfaces;

namespace ConsoleUI.Services.InputServices
{
    public class InputServiceFactory : IInputServiceFactory
    {
        private readonly StringProcessorInputService _stringSvc;
        private readonly UserProcessorInputService _userSvc;

        public InputServiceFactory(StringProcessorInputService stringSvc, UserProcessorInputService userSvc)
        {
            _stringSvc = stringSvc;
            _userSvc = userSvc;
        }

        public IInputService Get(MainMenuOption option) =>
          option switch
          {
              MainMenuOption.StringProcessor => _stringSvc,
              MainMenuOption.CreateUser => _userSvc,
              _ => throw new ArgumentOutOfRangeException(nameof(option))
          };
    }
}
