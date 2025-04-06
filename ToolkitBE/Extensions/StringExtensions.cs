namespace ToolkitBE.Extensions
{
    public static class StringExtensions
    {
        public static List<string> CreateList(this string userInput)
        {
            return userInput.Split(" ").ToList();
        }
    }
}
