namespace Website.Personal.Client.Profile.Resumes
{
    public static class ResumeExtensions
    {
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
            result += years > 0 ? $"{years} years " : "";
            result += months > 0 ? $"{months} months" : "";
            return result;
        }
    }
}