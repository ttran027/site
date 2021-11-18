using FluentResults;
using MediatR;

namespace Website.Personal.Client.Crypto.Prices
{
    public class CryptoPriceQuery : IRequest<Result<CryptoPrice>>
    {
        public string Ticker { get; set; } = null!;
    }
}