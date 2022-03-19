namespace Client.Components.Crypto.PricesTable
{
    public partial class CryptoPriceComponent
    {
        [Parameter]
        [EditorRequired]
        public CryptoPrice Price { get; set; } = null!;

        private RenderFragment GetPercent(double? p) => builder =>
        {
            if (p is not null)
            {
                var price = p >= 0 ? $"+{p:N2}" : p?.ToString("N2");
                builder.OpenElement(0, "span");
                var color = p < 0 ? "red-text" : "green-text";
                builder.AddAttribute(1, "class", color);
                builder.AddContent(2, price + "%");
                builder.CloseElement();
            }     
        };
    }
}