namespace MiniRedis.Common.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string value, string delimiter)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(delimiter))
                return value;

            var position = value.IndexOf(delimiter);
            if (position > -1)
                return value.Substring(0, position);

            return value;
        }
    }
}
