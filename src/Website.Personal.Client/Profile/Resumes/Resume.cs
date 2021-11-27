namespace Website.Personal.Client.Profile.Resumes
{
    public record Resume
    (
        string Summary,
        string FullName,
        string Email,
        List<string> Skills,
        List<Education> Educations,
        List<WorkExperience> Experiences
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
        DateTime? EndDate,
        string[] Responsibitites
    );

    public record Location
    (
        string City,
        string State
    );
}