using Client.Components.Crypto;

namespace Client.Components.Crypto.Prices
{
    public record CryptoPrice
    (
        CryptoInfo Crypto,
        string Base,
        double Amount
    );
}