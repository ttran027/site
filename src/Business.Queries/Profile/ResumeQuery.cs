using FluentResults;
using MediatR;
using System.Net.Http.Json;

namespace Business.Queries.Profile;

public class ResumeQuery : IRequest<Result<ResumeDetails>>
{
}

public class ResumeQueryHandler : IRequestHandler<ResumeQuery, Result<ResumeDetails>>
{
    private readonly ProfileQuerySettings _profileSettings;
    public ResumeQueryHandler(ProfileQuerySettings profileSettings)
    {
        _profileSettings = profileSettings;
    }

    public async Task<Result<ResumeDetails>> Handle(ResumeQuery request, CancellationToken cancellationToken)
    {
        var client = new HttpClient();

        return await Result.Try(async () =>
        {
            var response = await client.GetFromJsonAsync<ResumeDetails>(_profileSettings.ResumeFilename);
            if (response == null)
            {
                throw new Exception("Not Found");
            }
            return response;
        },
        ex => new Error("Failed to get resume " + ex.Message));
    }
}