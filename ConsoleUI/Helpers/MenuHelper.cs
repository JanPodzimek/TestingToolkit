using Spectre.Console;
public static class MenuHelper
{
    public static TEnum ShowMenu<TEnum>(string title, Dictionary<string, TEnum> options) where TEnum : Enum
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle("green")
                .AddChoices(options.Keys));

        return options[choice];
    }
}
