using System.Net.Http.Json;

namespace Client.CQRS.Queries
{
    public class CryptoAssetsQuery : IRequest<Result<IReadOnlyCollection<CryptoAsset>>>
    {
    }

    public class CryptoAssetsQueryHandler : IRequestHandler<CryptoAssetsQuery, Result<IReadOnlyCollection<CryptoAsset>>>
    {
        private readonly HttpClient _httpClient;

        public CryptoAssetsQueryHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("local");
        }

        public async Task<Result<IReadOnlyCollection<CryptoAsset>>> Handle(CryptoAssetsQuery request, CancellationToken cancellationToken)
        {
            return await Result.Try(async () =>
            {
                var response = await _httpClient.GetFromJsonAsync<IReadOnlyCollection<CryptoAsset>>("cryptos.json");
                if (response == null)
                {
                    throw new Exception("Not Found");
                }
                return response;
            },
            ex => new Error("Failed to get crypto assets: " + ex.Message));
        }
    }

    public record CryptoAsset
    (
        string Symbol,
        string Name
    );
}