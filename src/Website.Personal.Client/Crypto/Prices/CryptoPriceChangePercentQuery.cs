namespace Website.Personal.Client.Crypto.Prices
{
    public class CryptoPriceChangePercentQuery : IRequest<Result<string>>
    {
        public string Ticker { get; set; }
    }
}