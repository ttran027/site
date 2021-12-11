namespace Client.Common
{
    public static class Ultilities
    {
        public static string ToPrice(this double p)
        {
            return p > 0.0001 ? $"{p:N2}" : $"{p:f6}";
        }

        public static int[] PageSize(int size) => new[] { size };

        public static DateTime EnsureStartOfMonth(this DateTime date)
            => new(date.Year, date.Month, 1);

        public static RenderFragment ToPercent(this decimal p, Typo? typo) => builder =>
        {
            var price = p >= 0 ? $"+{p:N2}" : p.ToString("N2");
            builder.OpenComponent<MudText>(0);
            var color = p < 0 ? "red-text" : "green-text";
            builder.AddAttribute(1, "Class", color);
            builder.AddAttribute(2, "Typo", typo);
            builder.AddAttribute(3,"ChildContent", (RenderFragment)(builder2 => { builder2.AddContent(4,price + "%"); }));
            builder.CloseComponent();        
        };
    }
}
