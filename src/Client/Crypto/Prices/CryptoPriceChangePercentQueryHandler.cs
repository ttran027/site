using System.Net.Http.Json;

namespace Client.Crypto.Prices
{
    public class CryptoPriceChangePercentQueryHandler : IRequestHandler<CryptoPriceChangePercentQuery, Result<string>>
    {
        private readonly HttpClient _binance;

        public CryptoPriceChangePercentQueryHandler(IHttpClientFactory httpClientFactory)
        {
            _binance = httpClientFactory.CreateClient("binance");
        }

        public async Task<Result<string>> Handle(CryptoPriceChangePercentQuery request, CancellationToken cancellationToken)
        {
            return await Result.Try(async () =>
            {
                var binanceResponse = await _binance.GetFromJsonAsync<BinanceCryptoPriceResponse>($"ticker/24hr?symbol={request.Ticker.ToUpper()}USDT");
                return binanceResponse.PriceChangePercent;
            },
           ex => new Error("Failed to get percent price change " + request.Ticker + ": " + ex.Message));
        }
    }

    internal class BinanceCryptoPriceResponse
    {
        public string PriceChangePercent { get; set; }
    }
}
