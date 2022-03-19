namespace Client.Components.Crypto.PricesTable.Effects;

public class SearchTickerEffect : Effect<PriceTableActions.SearchTickers>
{ 
    public override Task HandleAsync(PriceTableActions.SearchTickers action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new PriceTableActions.GetPrices());
        return Task.CompletedTask;
    }
}