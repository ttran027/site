using Client.Contract.Constants;

namespace Client.Contract.Models.Crypto;

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
    public string GetKey() => $"{CryptoConstants.CachePrefix}{Symbol}";
};

public record CryptoInfo
(
    string Symbol,
    string? Name
);