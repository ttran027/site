using System.Net.Http.Json;
using Client.Shared;
using Client.Shared.Models;
using Client.Shared.Services;

namespace Client.Services;

public class SoccerService : ISoccerService
{
    private readonly ApplicationSettings _settings;
    public SoccerService(ApplicationSettings settings)
    {
        _settings = settings;
    }
    
    public async Task<Result<League>> GetLeagueData()
    {
        var client = new HttpClient();
        return await Result.Try(async () =>
            {
                var response = await client.GetFromJsonAsync<Response>(new Uri(_settings.SoccerUrl));
                if (response == null || !response.Data.Any())
                {
                    throw new Exception("Not Found");
                }
                return new League(
                    "England Premier Leuage",
                        response.Data.First().Season,
                    response.Data.Select(e => new Match
                        (
                            new Team(
                                e.Home_name,
                                e.HomeGoals,
                                e.Team_a_corners,
                                e.Team_a_shotsOnTarget,
                                e.Team_a_shotsOnTarget,
                                e.Team_a_possession,
                                e.Team_a_yellow_cards,
                                e.Team_a_red_cards,
                                e.Team_a_dangerous_attacks,
                                e.Team_a_fouls,
                                e.Team_a_freekicks,
                                e.Team_a_goalkicks
                                ),
                            new Team(
                                e.Away_name,
                                e.AwayGoals,
                                e.Team_b_corners,
                                e.Team_b_shotsOnTarget,
                                e.Team_b_shotsOnTarget,
                                e.Team_b_possession,
                                e.Team_b_yellow_cards,
                                e.Team_b_red_cards,
                                e.Team_b_dangerous_attacks,
                                e.Team_b_fouls,
                                e.Team_b_freekicks,
                                e.Team_b_goalkicks
                            ),
                            Ultilities.UnixTimeStampToDateTime(e.Date_unix)
                        )).ToList())
                    ;
            },
            ex => new Error("Failed to get resume " + ex.Message));
    }

    private class Response
    {
        public DataItem[] Data { get; set; }
        
        public class DataItem
        {
            public string Home_name { get; set; }
            public string Away_name { get; set; }
            public string Season { get; set; }
            public string Status { get; set; }
            public string[] HomeGoals { get; set; }
            public string[] AwayGoals { get; set; }
            public int Team_a_corners { get; set; }
            public int Team_b_corners { get; set; }
            public int Team_a_yellow_cards { get; set; }
            public int Team_b_yellow_cards { get; set; }
            public int Team_a_red_cards { get; set; }
            public int Team_b_red_cards { get; set; }
            public int Team_a_shotsOnTarget { get; set; }
            public int Team_b_shotsOnTarget { get; set; }
            public int Team_a_shotsOffTarget { get; set; }
            public int Team_b_shotsOffTarget { get; set; }
            public int Team_a_fouls { get; set; }
            public int Team_b_fouls { get; set; }
            public int Team_a_possession { get; set; }
            public int Team_b_possession { get; set; }
            public long Date_unix { get; set; }
            public int Team_a_freekicks { get; set; }
            public int Team_b_freekicks { get; set; }
            public int Team_a_goalkicks { get; set; }
            public int Team_b_goalkicks { get; set; }
            public int Team_a_dangerous_attacks { get; set; }
            public int Team_b_dangerous_attacks { get; set; }
        }
    }
}