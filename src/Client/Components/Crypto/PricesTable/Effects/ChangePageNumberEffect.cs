namespace Client.Components.Crypto.PricesTable.Effects;

public class ChangePageNumberEffect : Effect<PriceTableActions.ChangePageNumber>
{ 
    public override Task HandleAsync(PriceTableActions.ChangePageNumber action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new PriceTableActions.GetPrices());
        return Task.CompletedTask;
    }
}