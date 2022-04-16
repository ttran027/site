using Client.CryptoPrices.Models;
using FluentResults;
using System.Net.Http.Json;

namespace Client.CryptoPrices.PricesTable;

internal class BinancePriceFetcher
{
    private readonly List<CryptoInfo> _assets = CryptoAssets.Assets.ToList();
    private readonly HttpClient _httpClient;
    private const string Base = "USDT";
    public BinancePriceFetcher(string apiUrl = "https://api.binance.com/api/v3/")
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(apiUrl),
        };
    }

    public async Task<Result<List<CryptoPrice>>> GetPricesAsync(string[] symbols)
    {
        var list = new List<CryptoPrice>();
        foreach (var symbol in symbols)
        {
            var result = await GetPriceAsync(symbol);
            if (result.IsSuccess)
            {
                list.Add(result.Value);
            }
            else
            {
                return Result.Fail<List<CryptoPrice>>($"Failed to fetch price of {symbol}, {result.Errors.First()}");
            }
        }
        return Result.Ok(list);
    }


    public async Task<Result<CryptoPrice>> GetPriceAsync(string symbol)
    {
        try
        {
            var binanceResponse = await _httpClient.GetFromJsonAsync<BinanceCryptoPriceResponse>($"ticker/24hr?symbol={symbol.ToUpper()}{Base}");
            if (binanceResponse is null)
                throw new Exception("Unavailable");
            var price = GetModel(symbol, binanceResponse);

            if (price is not null)
                return Result.Ok(price);          
            else
                throw new Exception("Not supported");
        }
        catch (Exception e)
        {
            return Result.Fail<CryptoPrice>(e.Message);
        }
    }

    private CryptoPrice? GetModel(string symbol, BinanceCryptoPriceResponse price)
    {
        var info = _assets.FirstOrDefault(e => e.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase));
        if (info is not null)
        {
            return new CryptoPrice(symbol, info.Name, Base, double.Parse(price.LastPrice), double.Parse(price.PriceChangePercent), DateTime.Now);
        }
        return null;
    }
}