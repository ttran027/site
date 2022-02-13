using Business.Queries.Crypto;

namespace Business.Queries;

public class QuerySettings
{
    public CryptoQuerySettings CryptoSettings { get; set; } = new();
}