namespace Client.Components.Crypto.PricesTable
{
    public record CryptoPrice
    (
        string Symbol,
        string? Name,
        string Base,
        double? Price,
        double? PercentChange24Hour,
        DateTime LastUpdated
    );

    public record CryptoInfo
    (
        string Symbol,
        string? Name
    );
}