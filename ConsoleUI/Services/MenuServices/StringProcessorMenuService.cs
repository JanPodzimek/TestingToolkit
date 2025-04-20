using ConsoleUI.Services.InputServices;

public class StringProcessorMenuService : IMenuService<StringProcessorInputService.StringProcessorMenuOption>
{
    public StringProcessorInputService.StringProcessorMenuOption ShowMenu()
    {
        var options = new Dictionary<string, StringProcessorInputService.StringProcessorMenuOption>
        {
            { "✅ Modify list of values", StringProcessorInputService.StringProcessorMenuOption.StringMutator },
            { "✅ Generate string of specific length", StringProcessorInputService.StringProcessorMenuOption.StringGenerator },
            { "🔙 Return", StringProcessorInputService.StringProcessorMenuOption.Return }
        };

        return MenuHelper.ShowMenu("\n[yellow]Choose feature:[/]", options);
    }
}
