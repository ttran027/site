namespace Website.Personal.Client.Common
{
    public static class Ultilities
    {
        public static string ToPrice(this double p)
        {
            return p > 0.0001 ? $"${p:N}" : $"${p:f6}";
        }

        public static int[] PageSize(int size) => new[] { size };

        public static DateTime EnsureStartOfMonth(this DateTime date)
            => new(date.Year, date.Month, 1); 
    }
}
