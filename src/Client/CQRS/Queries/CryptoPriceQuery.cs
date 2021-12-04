using System.Net.Http.Json;

namespace Client.CQRS.Queries
{
    public class CryptoPriceQuery : IRequest<Result<BinanceCryptoPriceResponse>>
    {
        public string Symbol { get; set; }
        public string Base { get; set; }
    }

    public class CryptoPriceChangePercentQueryHandler : IRequestHandler<CryptoPriceQuery, Result<BinanceCryptoPriceResponse>>
    {
        private readonly HttpClient _binance;

        public CryptoPriceChangePercentQueryHandler(IHttpClientFactory httpClientFactory)
        {
            _binance = httpClientFactory.CreateClient("binance");
        }

        public async Task<Result<BinanceCryptoPriceResponse>> Handle(CryptoPriceQuery request, CancellationToken cancellationToken)
        {
            return await Result.Try(async () =>
            {
                var binanceResponse = await _binance.GetFromJsonAsync<BinanceCryptoPriceResponse>($"ticker/24hr?symbol={request.Symbol.ToUpper()}{request.Base.ToUpper()}");
                if (binanceResponse is null)
                {
                    throw new Exception("Unavailable");
                }
                return binanceResponse;
            },
           ex => new Error("Failed to get Binance price " + request.Symbol + ": " + ex.Message));
        }
    }

    public class BinanceCryptoPriceResponse
    {
        public string LastPrice { get; set; }
        public string PriceChangePercent { get; set; }
    }
}