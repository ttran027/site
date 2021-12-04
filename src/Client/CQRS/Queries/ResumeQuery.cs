using Client.Components.Resume;
using System.Net.Http.Json;

namespace Client.CQRS.Queries
{
    public class ResumeQuery : IRequest<Result<ResumeDetails>>
    {
    }

    public class ResumeQueryHandler : IRequestHandler<ResumeQuery, Result<ResumeDetails>>
    {
        private readonly HttpClient _httpClient;

        public ResumeQueryHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("local");
        }

        public async Task<Result<ResumeDetails>> Handle(ResumeQuery request, CancellationToken cancellationToken)
        {
            return await Result.Try(async () =>
            {
                var response = await _httpClient.GetFromJsonAsync<ResumeDetails>("resume.json");
                if (response == null)
                {
                    throw new Exception("Not Found");
                }
                return response;
            },
            ex => new Error("Failed to get resume " + ex.Message));
        }
    }
}