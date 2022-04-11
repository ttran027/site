using Client.Contract.Models.Crypto;
using Fluxor;

namespace Client.Contract.Stores;

public enum PriceTableStatus
{
    Pulling
}

public record PriceTableState
(
    IReadOnlyCollection<CryptoPrice> Items, 
    string SearchTerm,
    int PageNumber,
    bool Loading
)
{
    public bool CanGetData()
        => string.IsNullOrEmpty(SearchTerm)
           && PageNumber == 0
           && Loading is false;
};


public class PriceTableFeature : Feature<PriceTableState>
{
    public override string GetName() => "TableState";

    protected override PriceTableState GetInitialState()
    {
        return new PriceTableState(Array.Empty<CryptoPrice>(), string.Empty, 0, false);
    }
}

public abstract class PriceTableActions
{
    public record SearchTickers(string Term);

    public record ChangePageNumber(int PageNumber);

    public record GetPrices();

    public record GetPricesComplete(IReadOnlyCollection<CryptoPrice> Items);

    public record UpdatePrice(CryptoInfo Crypto);

    public record UpdatePriceComplete(CryptoPrice NewPrice);
}

public static class PriceTableReducers
{
    [ReducerMethod]
    public static PriceTableState OnSearch(PriceTableState state, PriceTableActions.SearchTickers action)
    {
        return state with
        {
            SearchTerm = action.Term,
        };
    }

    [ReducerMethod]
    public static PriceTableState OnChangePageNumber(PriceTableState state, PriceTableActions.ChangePageNumber action)
    {
        return state with
        {
            PageNumber = action.PageNumber,
        };
    }

    [ReducerMethod]
    public static PriceTableState OnGetPrices(PriceTableState state, PriceTableActions.GetPrices action)
    {
        return state with
        {
            Loading = true,
        };
    }

    [ReducerMethod]
    public static PriceTableState OnGetPricesComplete(PriceTableState state, PriceTableActions.GetPricesComplete action)
    {
        return state with
        {
            Loading = false,
            Items = action.Items,
        };
    }
}
