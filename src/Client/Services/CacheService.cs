using Blazored.LocalStorage;
using Client.Contract.Interfaces;

namespace Client.Services;

public class CacheService : ICacheService
{
    private readonly ILocalStorageService _localStorage;

    public CacheService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<Result<T>> GetAsync<T>(string key)
    {
        try
        {
            var data = await _localStorage.GetItemAsync<T>(key);
            if (data is not null)
            {
                return Result.Ok(data);
            }

            return Result.Fail<T>("Not found.");          
        }
        catch (Exception e)
        {
            return Result.Fail<T>(e.Message);
        }
    }

    public async Task<Result<IReadOnlyCollection<string>>> GetKeysAsync(Func<string, bool> predicate)
    {
        try
        {
            var keys = (IReadOnlyCollection<string>) (await _localStorage.KeysAsync()).Where(predicate).ToList();
            return Result.Ok(keys);
        }
        catch (Exception e)
        {
            return Result.Fail<IReadOnlyCollection<string>>(e.Message);
        }
    }

    public async Task<Result> SaveAsync<T>(string key, T data)
    {
        try
        {
            await _localStorage.SetItemAsync<T>(key, data);
            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }
}