using FluentResults;

namespace Client.Contract.Interfaces;

public interface ICacheService
{
    Task<Result> SaveAsync<T>(string key, T data);

    Task<Result<T>> GetAsync<T>(string key);

    Task<Result<IReadOnlyCollection<string>>> GetKeysAsync(Func<string, bool> predicate);
}