namespace Client.Common;

public record NavMenuItem
(
    string Title,
    string Href,
    string Icon,
    bool IsHome
);