namespace Client.Shared.Models;

public record League(string Name, string Season, List<Match> Matches);

public record Match
(
    Team Home,
    Team Away,
    DateTime KickOffTime
);

public record Team
(
    string Name,
    string[] Goals,
    int Corners,
    int ShotsOnTarget,
    int ShotsOffTarget,
    int Possession,
    int YellowCards,
    int RedCards,
    int DangerousAttacks,
    int Fouls,
    int FreeKicks,
    int GoalKicks
);