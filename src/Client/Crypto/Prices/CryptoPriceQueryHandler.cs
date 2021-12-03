using System.Net.Http.Json;

namespace Client.Crypto.Prices
{
    public class CryptoPriceQueryHandler : IRequestHandler<CryptoPriceQuery, Result<CryptoPrice>>
    {
        private readonly HttpClient _coinbase;


        public CryptoPriceQueryHandler(IHttpClientFactory httpClientFactory)
        {
            _coinbase = httpClientFactory.CreateClient("coinbase");
        }

        public async Task<Result<CryptoPrice>> Handle(CryptoPriceQuery request, CancellationToken cancellationToken)
        {
            return await Result.Try(async () =>
            {
                var coinbaseResponse = await _coinbase.GetFromJsonAsync<CoinbaseCryptoPriceResponse>($"prices/{request.CryptoInfo.Ticker}-usd/spot");
                return new CryptoPrice(request.CryptoInfo,
                    coinbaseResponse.Data.Currency,
                    double.Parse(coinbaseResponse.Data.Amount));
            },
            ex => new Error("Failed to get price of " + request.CryptoInfo.Ticker + ": " + ex.Message));
        }
    }

    internal class CoinbaseCryptoPriceData
    {
        public string Base { get; set; }

        public string Currency { get; set; }

        public string Amount { get; set; }
    }

    internal class CoinbaseCryptoPriceResponse
    {
        public CoinbaseCryptoPriceData Data { get; set; }
    }
}