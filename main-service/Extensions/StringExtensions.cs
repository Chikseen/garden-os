namespace ExtensionMethods
{
    public static class MyExtensions
    {
        public static string Clean(this string str)
        {
            return str.Replace("\n", " ").Replace("\r", " ").Replace("\t", " ");
        }
    }
}