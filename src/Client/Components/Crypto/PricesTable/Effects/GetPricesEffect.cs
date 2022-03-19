using Business.Queries.Crypto;

namespace Client.Components.Crypto.PricesTable.Effects;

public class GetPricesEffect : Effect<PriceTableActions.GetPrices>
{
    private readonly IMediator _mediator;
    private readonly IState<PriceTableState> _state;
    private readonly List<CryptoInfo> _assets = CryptoAssets.Assets.Select(e => new CryptoInfo(e.Symbol, e.Name)).ToList();
    private readonly string Base = "USDT";

    public GetPricesEffect(IMediator mediator, IState<PriceTableState> state)
    {
        _mediator = mediator;
        _state = state;
    }

    public override async Task HandleAsync(PriceTableActions.GetPrices action, IDispatcher dispatcher)
    {
        var state = _state.Value;
        var filteredAssets = _assets.Where(Filter);
        var pageAssets = filteredAssets.Skip(state.PageSize * state.PageNumber).Take(state.PageSize);
        var items = new List<CryptoPrice>();
        foreach (var asset in pageAssets)
        {
            var p = await GetPriceAsync(asset.Symbol);
            items.Add(p);
        }

        dispatcher.Dispatch(new PriceTableActions.GetPricesComplete(items, filteredAssets.Count()));
    }

    private async Task<CryptoPrice> GetPriceAsync(string symbol)
    {

        var result = await _mediator.Send(new CryptoPriceQuery(symbol, Base));
        if (result.IsSuccess)
        {
            return new CryptoPrice(symbol, Base, double.Parse(result.Value.LastPrice), double.Parse(result.Value.PriceChangePercent));
        }
        return new CryptoPrice(symbol, Base, null, null);
    }

    private bool Filter(CryptoInfo info)
    {
        if (!string.IsNullOrEmpty(_state.Value.SearchTerm))
        {
            return info.Name.Contains(_state.Value.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    info.Symbol.Contains(_state.Value.SearchTerm, StringComparison.OrdinalIgnoreCase);
        }
        return true;
    }
}