namespace Client.Components.Crypto.PricesTable
{ 
    public record PriceTableState
    (
        string SearchTerm,
        int PageNumber
    );

    
    public class PriceTableFeature : Feature<PriceTableState>
    {
        public override string GetName() => "TableState";

        protected override PriceTableState GetInitialState()
        {
            return new PriceTableState(string.Empty, 0);
        }
    }

    public abstract class PriceTableActions
    {
        public record SearchTickers(string Term);

        public record ChangePageNumber(int PageNumber);
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
    }
}