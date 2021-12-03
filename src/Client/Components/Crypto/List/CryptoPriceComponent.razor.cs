using Client.Components.Crypto.Prices;

namespace Client.Components.Crypto.List
{
    public partial class CryptoPriceComponent
    {
        private CryptoPrice? Price;
        private string? PriceChangePercent;
        private bool _loading;

        [Parameter]
        [EditorRequired]
        public CryptoInfo CryptoInfo { get; set; } = null!;

        [Inject]
        private IMediator Mediator { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            _loading = true;

            var price = await Mediator.Send(new CryptoPriceQuery { CryptoInfo = CryptoInfo });
            Price = price.IsSuccess ? price.Value : null;

            var percent = await Mediator.Send(new CryptoPriceChangePercentQuery { Ticker = CryptoInfo.Ticker });
            PriceChangePercent = percent.IsSuccess ? percent.Value : null;

            _loading = false;
        }
    }
}