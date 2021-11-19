namespace Website.Personal.Client.Crypto.Prices
{
    public class CryptoPriceQuery : IRequest<Result<CryptoPrice>>
    {
        public CryptoInfo CryptoInfo { get; set; } = null!;
    }
}