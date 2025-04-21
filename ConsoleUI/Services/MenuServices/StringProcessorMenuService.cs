using ConsoleUI.Enums;

public class StringProcessorMenuService : IMenuService<StringProcessorMenuOption>
{
    public StringProcessorMenuOption ShowMenu()
    {
        var options = new Dictionary<string, StringProcessorMenuOption>
        {
            { "✅ Modify list of values", StringProcessorMenuOption.StringMutator },
            { "✅ Generate string of specific length", StringProcessorMenuOption.StringGenerator },
            { "🔙 Return", StringProcessorMenuOption.Return }
        };

        return MenuHelper.ShowMenu("\n[yellow]Choose feature:[/]", options);
    }
}
