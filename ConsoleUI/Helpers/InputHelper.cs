namespace ConsoleUI.Helpers
{
    public static class InputHelper
    {
        public static void End()
        {
            Console.WriteLine();
            Console.WriteLine("Press any to key to continue...");
            Console.ReadKey(true);
        }
    }
}
