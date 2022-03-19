namespace Client.Components.Crypto.PricesTable
{
    public record PriceTableState
    (
        string SearchTerm,
        int PageNumber,
        int PageSize,
        int Total,
        IEnumerable<CryptoPrice> Items,
        bool Loading
    )
    {
        public bool CanGetData()
            => string.IsNullOrEmpty(SearchTerm)
               && PageNumber == 0
               && Items.Count() == 0
               && Total == 0
               && Loading is false;
    };

    
    public class PriceTableFeature : Feature<PriceTableState>
    {
        public override string GetName() => "TableState";

        protected override PriceTableState GetInitialState()
        {
            return new PriceTableState(string.Empty, 0, 5, 0, Array.Empty<CryptoPrice>(), false);
        }
    }

    public abstract class PriceTableActions
    {
        public record SearchTickers(string Term);

        public record ChangePageNumber(int PageNumber);

        public record GetPrices();

        public record GetPricesComplete(IEnumerable<CryptoPrice> Items, int Total);
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
                Total = action.Total,
            };
        }
    }
}