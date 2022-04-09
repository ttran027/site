using Fluxor;
using MediatR;

namespace Client.CryptoPrices.PricesTable.Effects;

public class UpdatePriceEffect : Effect<PriceTableActions.UpdatePrice>
{
    private readonly IMediator _mediator;
    private readonly string Base = "USDT";
    private readonly ICryptoPriceCache _cache;

    public UpdatePriceEffect(IMediator mediator, ICryptoPriceCache cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    public override async Task HandleAsync(PriceTableActions.UpdatePrice action, IDispatcher dispatcher)
    {

        var p = await GetPriceAsync(action.Crypto);
        await _cache.UpdatePriceAsync(p);
        dispatcher.Dispatch(new PriceTableActions.UpdatePriceComplete(p));
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