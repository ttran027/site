namespace Client.Components.Crypto.Prices
{
    public class CryptoPriceChangePercentQuery : IRequest<Result<string>>
    {
        public string Ticker { get; set; }
    }
}