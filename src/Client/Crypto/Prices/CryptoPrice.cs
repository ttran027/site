namespace Client.Crypto.Prices
{
    public record CryptoPrice
    (
        CryptoInfo Crypto,
        string Base,
        double Amount
    );
}