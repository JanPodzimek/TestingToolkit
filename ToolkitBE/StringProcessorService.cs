namespace ToolkitBE
{
    public class StringProcessorService
    {
        public List<string> ApplyDelimiter(List<string> data, string delimiter = ",")
        {
            return data.Select((item, index) =>
                index != data.Count - 1
                    ? item + delimiter
                    : item
            ).ToList();
        }

        public List<string> ApplySingleQuotes(List<string> data)
        {
            return data.Select(item => "'" + item + "'").ToList();
        }

        public List<string> ApplyDoubleQuotes(List<string> data)
        {
            return data.Select(item => "\"" + item + "\"").ToList();
        }

        public List<string> RemoveDuplicates(List<string> data)
        {
            return data.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        }
    }
}
