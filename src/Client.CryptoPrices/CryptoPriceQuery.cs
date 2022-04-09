using FluentResults;
using MediatR;
using System.Net.Http.Json;

namespace Client.CryptoPrices;

public record BinanceCryptoPriceResponse
(
    string LastPrice,
    string PriceChangePercent
);

public record CryptoPriceQuery
(
    string Symbol,
    string Base
) : IRequest<Result<BinanceCryptoPriceResponse>>
{

}

public class CryptoPriceChangePercentQueryHandler : IRequestHandler<CryptoPriceQuery, Result<BinanceCryptoPriceResponse>>
{
    private readonly CryptoQuerySettings _cryptoSettings;
    public CryptoPriceChangePercentQueryHandler()
    {
        _cryptoSettings = new CryptoQuerySettings()
        {
            BinanceAddress = "https://api.binance.com/api/v3/",
        };
    }

    public async Task<Result<BinanceCryptoPriceResponse>> Handle(CryptoPriceQuery request, CancellationToken cancellationToken)
    {
        var client = new HttpClient
        {
            BaseAddress = new Uri(_cryptoSettings.BinanceAddress),
        };

        return await Result.Try(async () =>
        {
            var binanceResponse = await client.GetFromJsonAsync<BinanceCryptoPriceResponse>($"ticker/24hr?symbol={request.Symbol.ToUpper()}{request.Base.ToUpper()}");
            if (binanceResponse is null)
            {
                throw new Exception("Unavailable");
            }
            return binanceResponse;
        },
        ex => new Error("Failed to get Binance price " + request.Symbol + ": " + ex.Message));
    }
}