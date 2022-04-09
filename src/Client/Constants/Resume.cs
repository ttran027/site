using Client.Shared;
using System.Net.Http.Json;

namespace Client.Constants;

public static class Resume
{
    public static async Task<Result<ResumeDetails>> GetAsync(string url) 
    { 
        var client = new HttpClient();
        return await Result.Try(async () =>
        {
            var response = await client.GetFromJsonAsync<ResumeDetails>(new Uri(url));
            if (response == null)
            {
                throw new Exception("Not Found");
            }
            return response;
        },
            ex => new Error("Failed to get resume " + ex.Message));
    }

    public static string ToStartEndMonths(this WorkExperience workExperience)
    {
        var endDate = workExperience.EndDate is null ? "Current" : DateTime.Today.ToString("MMMM yyyy");
        return $"{workExperience.StartDate:MMMM yyyy} - {endDate}";
    }

    public static string ToMonthsYearsLength(this WorkExperience workExperience)
    {
        var referenceDate = DateTime.MinValue + ((workExperience.EndDate ?? DateTime.Today) - workExperience.StartDate.EnsureStartOfMonth());
        var years = referenceDate.Year - 1;
        var months = referenceDate.Month - 1;
        var result = string.Empty;
        result += years > 0 ? $"({years} years )" : "";
        result += months > 0 ? $"({months} months)" : "";
        return result;
    }
}

public record ResumeDetails
    (
        string Summary,
        string FullName,
        string Email,
        List<string> Skills,
        List<Education> Educations,
        List<WorkExperience> Experiences,
        Contact Contact
    );

public record Education
(
    string Institution,
    Location Location,
    string Degree,
    DateTime GraduationDate,
    decimal Grade
);
public record WorkExperience
(
    string Company,
    Location Location,
    string Position,
    DateTime StartDate,
    DateTime? EndDate
);

public record Location
(
    string City,
    string State
);

public record Contact
(
    string Email,
    string Linkedin
);
