namespace ExtensionMethods
{
    public static class MyExtensions
    {
        public static String Clean(this string str)
        {
            return str.Replace("\n", "").Replace("\r", "").Replace("\t", "");
        }
    }
}