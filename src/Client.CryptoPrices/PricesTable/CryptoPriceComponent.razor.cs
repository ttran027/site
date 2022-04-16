using Client.Contract.Interfaces;
using Client.CryptoPrices.Models;
using Microsoft.AspNetCore.Components;

namespace Client.CryptoPrices.PricesTable
{
    public partial class CryptoPriceComponent : IDisposable
    {
        private Timer? _timer;
        private readonly BinancePriceFetcher _fetcher = new();
        private CryptoPrice? Price;
        private string Key => CryptoConstants.CacheKey(Crypto.Symbol);

        [Parameter]
        [EditorRequired]
        public CryptoInfo Crypto { get; set; } = null!;

        [Inject]
        private ICacheService CacheService { get; set; } = null!;

        public void Dispose()
        {
            _timer?.Dispose();
        }

        protected async override Task OnInitializedAsync()
        {
            var cache = await CacheService.GetAsync<CryptoPrice>(Key);

            if (cache.IsSuccess)
            {
                Price = cache.Value;
            }
            else
            {
                var p1 = await _fetcher.GetPriceAsync(Crypto.Symbol);

                if (p1.IsSuccess)
                {
                    await CacheService.SaveAsync(Key, p1.Value);
                    Price = p1.Value;
                }
            }
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _timer = new Timer(UpdatePrice, null, 0, 10000);
            }
            return base.OnAfterRenderAsync(firstRender);
        }

        private async void UpdatePrice(object? state)
        {
            if (Price is not null && (DateTime.Now - Price.LastUpdated).TotalSeconds > 10)
            {
                var p = await _fetcher.GetPriceAsync(Crypto.Symbol);

                if (p.IsSuccess)
                {
                    await CacheService.SaveAsync(Key, p.Value);
                    StateHasChanged();
                }
            }
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