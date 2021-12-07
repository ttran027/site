namespace Client.Components.Crypto.PricesTable
{
    public partial class CryptoPriceComponent
    {
        private bool _loading;
        private CryptoPrice Price;

        [Parameter]
        [EditorRequired]
        public string Symbol { get; set; }

        [Parameter]
        [EditorRequired]
        public Func<string, Task<CryptoPrice>> GetPrice { get; set; }

        protected async override Task OnInitializedAsync()
        {
            _loading = true;
            Price = await GetPrice(Symbol);
            _loading = false;
        }

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