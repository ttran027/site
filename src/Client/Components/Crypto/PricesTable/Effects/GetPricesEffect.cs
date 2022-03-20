using Business.Queries.Crypto;
using Client.Cache;

namespace Client.Components.Crypto.PricesTable.Effects;

public class GetPricesEffect : Effect<PriceTableActions.GetPrices>
{
    private readonly IMediator _mediator;
    private readonly List<CryptoInfo> _assets = CryptoAssets.Assets.Select(e => new CryptoInfo(e.Symbol, e.Name)).ToList();
    private readonly string Base = "USDT";
    private readonly ICryptoPriceCache _cache;

    public GetPricesEffect(IMediator mediator, ICryptoPriceCache cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    public override async Task HandleAsync(PriceTableActions.GetPrices action, IDispatcher dispatcher)
    {
        
         await GetPricesFromRemote();
               
        dispatcher.Dispatch(new PriceTableActions.GetPricesComplete());
    }

    private async Task GetPricesFromRemote()
    {
        var items = new List<CryptoPrice>();

        foreach (var item in _assets)
        {
            var p = await GetPriceAsync(item);
            items.Add(p);
        }

        await _cache.SavePricesAsync(items);
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
}