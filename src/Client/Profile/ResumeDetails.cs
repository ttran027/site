﻿namespace Business.Queries.Profile;

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