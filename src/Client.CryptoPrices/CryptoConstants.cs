namespace Client.CryptoPrices;

internal static class CryptoConstants
{
    public static string CacheKey(string id) => "heima.crypto.price." + id;
}