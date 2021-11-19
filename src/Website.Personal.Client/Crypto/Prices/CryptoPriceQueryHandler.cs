using System.Net.Http.Json;

namespace Website.Personal.Client.Crypto.Prices
{
    public class CryptoPriceQueryHandler : IRequestHandler<CryptoPriceQuery, Result<CryptoPrice>>
    {
        private readonly HttpClient _httpClient;

        public CryptoPriceQueryHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("coinbase");
        }

        public async Task<Result<CryptoPrice>> Handle(CryptoPriceQuery request, CancellationToken cancellationToken)
        {
            return await Result.Try(async () =>
            {
                var response = await _httpClient.GetFromJsonAsync<CoinbaseCryptoPriceResponse>($"prices/{request.CryptoInfo.Ticker}-usd/sell");
                return new CryptoPrice(request.CryptoInfo, response.Data.Currency, double.Parse(response.Data.Amount));
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