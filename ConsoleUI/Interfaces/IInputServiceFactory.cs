using ConsoleUI.Enums;

namespace ConsoleUI.Interfaces
{
    public interface IInputServiceFactory
    {
        IInputService Get(MainMenuOption option);
    }
}
