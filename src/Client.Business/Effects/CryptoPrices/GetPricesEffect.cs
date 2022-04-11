using Client.Business.Queries;
using Client.Contract.Constants;
using Client.Contract.Interfaces;
using Client.Contract.Models.Crypto;
using Client.Contract.Stores;
using Fluxor;
using MediatR;

namespace Client.Business.Effects.CryptoPrices;

public class GetPricesEffect : Effect<PriceTableActions.GetPrices>
{
    private readonly IMediator _mediator;
    private readonly List<CryptoInfo> _assets = CryptoAssets.Assets.Select(e => new CryptoInfo(e.Symbol, e.Name)).ToList();
    private readonly string Base = "USDT";
    private readonly ICacheService _cache;

    public GetPricesEffect(IMediator mediator, ICacheService cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    public override async Task HandleAsync(PriceTableActions.GetPrices action, IDispatcher dispatcher)
    {
        List<CryptoPrice> items;
        if (!await IsCacheValid())
        {
            items = await GetPricesFromRemote();
        }
        else
        {
            items = await GetPricesFromCacheAsync();
        }

        dispatcher.Dispatch(new PriceTableActions.GetPricesComplete(items));
    }

    private async Task<bool> IsCacheValid()
    {
        var keys = await _cache.GetKeysAsync(IsCryptoPriceKey);

        return keys.IsSuccess && keys.Value.Count == CryptoAssets.Assets.Count;
    }

    private bool IsCryptoPriceKey(string key)
       => key.Contains(CryptoConstants.CachePrefix, StringComparison.OrdinalIgnoreCase);

    private async Task<List<CryptoPrice>> GetPricesFromRemote()
    {
        var items = new List<CryptoPrice>();
        var keysInCache = await _cache.GetKeysAsync(IsCryptoPriceKey);
        if (keysInCache.IsSuccess)
        {
            foreach (var item in _assets)
            {
                if (!keysInCache.Value.Any(x => x.Contains(item.Symbol, StringComparison.OrdinalIgnoreCase)))
                {
                    var p = await GetPriceAsync(item);
                    items.Add(p);
                }
            }

            await SavePricesAsync(items);
        }
        return items;
    }

    private async Task SavePricesAsync(IReadOnlyCollection<CryptoPrice> items)
    {
        foreach (var item in items)
        {
            await _cache.SaveAsync(item.GetKey(), item);
        }
    }

    private async Task<CryptoPrice> GetPriceAsync(CryptoInfo c)
    {
        var result = await _mediator.Send(new CryptoPriceQuery(c.Symbol, Base));
        if (result.IsSuccess)
        {
            return new CryptoPrice(c.Symbol, c.Name, Base, double.Parse(result.Value.LastPrice), double.Parse(result.Value.PriceChangePercent), DateTime.Now);
        }
        return new CryptoPrice(c.Symbol, c.Name, Base, null, null, DateTime.Now);
    }

    private async Task<List<CryptoPrice>> GetPricesFromCacheAsync()
    {
        var keys = await _cache.GetKeysAsync(IsCryptoPriceKey);
        var items = new List<CryptoPrice>();
        if (keys.IsSuccess)
        {
            foreach (var key in keys.Value)
            {
                var data = await _cache.GetAsync<CryptoPrice>(key);
                if (data.IsSuccess)
                {
                    items.Add(data.Value);
                }
            }
        }
        return items;
    }
}