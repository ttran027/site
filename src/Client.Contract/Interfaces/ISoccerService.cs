using Client.Shared.Models;
using FluentResults;

namespace Client.Shared.Services;

public interface ISoccerService
{
    Task<Result<League>> GetLeagueData();
}