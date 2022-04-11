using Client.Contract.Models.Crypto;
using Client.Contract.Stores;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace Client.CryptoPrices.PricesTable
{
    public partial class CryptoPriceComponent : IDisposable
    {
        private Timer? _timer;
        [Parameter]
        [EditorRequired]
        public CryptoPrice Price { get; set; } = null!;

        [Inject]
        private IDispatcher Dispatcher { get; set; } = null!;

        public void Dispose()
        {
            _timer?.Dispose();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (!firstRender) return;

            _timer = new Timer(UpdatePrice, null, 0, 10000);

            base.OnAfterRender(firstRender);
        }

        private void UpdatePrice(object? state)
        {
            if ((DateTime.Now - Price.LastUpdated).TotalSeconds > 10)
            {
                Dispatcher.Dispatch(new PriceTableActions.UpdatePrice(new CryptoInfo(Price.Symbol, Price.Name)));
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