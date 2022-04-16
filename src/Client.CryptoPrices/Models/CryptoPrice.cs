namespace Client.CryptoPrices.Models;

public record CryptoPrice
(
    string Symbol,
    string? Name,
    string Base,
    double? Price,
    double? PercentChange24Hour,
    DateTime LastUpdated
)
{
    public string GetKey() => CryptoConstants.CacheKey(Symbol);
};

public record CryptoInfo
(
    string Symbol,
    string Name
);