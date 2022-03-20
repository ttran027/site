using Blazored.LocalStorage;
using Client.Components.Crypto.PricesTable;

namespace Client.Cache;

public interface ICryptoPriceCache
{
    Task<bool> IsEmptyAsync();
    Task SavePricesAsync(IReadOnlyCollection<CryptoPrice> items);
    Task<IReadOnlyCollection<CryptoPrice>> GetPricesAsync();
}

public class CryptoPriceCache : ICryptoPriceCache
{
    private readonly ILocalStorageService _localStorage;
    private readonly string Prefix = "heima.crypto.price.";
    public CryptoPriceCache(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<bool> IsEmptyAsync()
    {
        var keys = (await _localStorage.KeysAsync()).Where(IsCryptoPriceKey);
        return !keys.Any();
    }

    public async Task SavePricesAsync(IReadOnlyCollection<CryptoPrice> items)
    {
        foreach (var item in items)
        {
            await _localStorage.SetItemAsync<CryptoPrice>($"{Prefix}{item.Symbol}", item);
        }
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
}