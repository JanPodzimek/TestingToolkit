namespace StringProcessor
{
    public static class Generator
    {
        public static string GenerateString(int length)
        {
            return new string('X', length);
        }
    }
}
