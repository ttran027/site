using Blazored.LocalStorage;
using Client.CryptoPrices.PricesTable;

namespace Client.CryptoPrices;

public interface ICryptoPriceCache
{
    Task<bool> IsCacheValid();
    Task SavePricesAsync(IReadOnlyCollection<CryptoPrice> items);
    Task<IReadOnlyCollection<CryptoPrice>> GetPricesAsync();
    Task UpdatePriceAsync(CryptoPrice item);
    Task<IReadOnlyCollection<string>> GetKeysInCache();
}

public class CryptoPriceCache : ICryptoPriceCache
{
    private readonly ILocalStorageService _localStorage;
    private readonly string Prefix = "heima.crypto.price.";
    public CryptoPriceCache(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<bool> IsCacheValid()
    {
        var keys = await GetKeysInCache();
        return keys.Count == CryptoAssets.Assets.Count;
    }

    public async Task SavePricesAsync(IReadOnlyCollection<CryptoPrice> items)
    {
        foreach (var item in items)
        {
            await UpdatePriceAsync(item);
        }
    }

    public async Task UpdatePriceAsync(CryptoPrice item)
    {
        await _localStorage.SetItemAsync($"{Prefix}{item.Symbol}", item);
    }

    public async Task<IReadOnlyCollection<CryptoPrice>> GetPricesAsync()
    {
        var keys = (await _localStorage.KeysAsync()).OrderBy(x => x);
        var items = new List<CryptoPrice>();
        foreach (var key in keys)
        {
            if (IsCryptoPriceKey(key))
            {
                var data = await _localStorage.GetItemAsync<CryptoPrice>(key);
                items.Add(data);
            }
        }
        return items;
    }

    private bool IsCryptoPriceKey(string key)
        => key.Contains(Prefix, StringComparison.OrdinalIgnoreCase);

    public async Task<IReadOnlyCollection<string>> GetKeysInCache()
    {
        return (await _localStorage.KeysAsync()).Where(IsCryptoPriceKey).ToList();
    }
}