using System.Net.Http.Json;

namespace Website.Personal.Client.Profile.Resumes
{
    public class ResumeQueryHandler : IRequestHandler<ResumeQuery, Result<Resume>>
    {
        private readonly HttpClient _httpClient;

        public ResumeQueryHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("local");
        }

        public async Task<Result<Resume>> Handle(ResumeQuery request, CancellationToken cancellationToken)
        {
            return await Result.Try(async () =>
            {
                var response = await _httpClient.GetFromJsonAsync<Resume>("resume.json");
                if(response == null)
                {
                    throw new Exception("Not Found");
                }
                return response;
            },
            ex => new Error("Failed to get resume "+ ex.Message));
        }
    }
}