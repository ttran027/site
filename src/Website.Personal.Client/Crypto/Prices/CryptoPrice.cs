namespace Website.Personal.Client.Crypto.Prices
{
    public record CryptoPrice
    (
        string Ticker,
        string Base,
        double Amount
    );
}