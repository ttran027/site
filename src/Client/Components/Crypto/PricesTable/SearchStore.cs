namespace Client.Components.Crypto.PricesTable
{ 
    public record SearchState
    (
        string SearchTerm
    );

    
    public class SearchFeature : Feature<SearchState>
    {
        public override string GetName() => "Search";

        protected override SearchState GetInitialState()
        {
            return new SearchState(string.Empty);
        }
    }

    public class SearchEnterAction
    {
        public string Term { get; set; }
    }

    public static class SearchReducers
    {
        [ReducerMethod]
        public static SearchState OnSearch(SearchState state, SearchEnterAction action)
        {
            return state with
            {
                SearchTerm = action.Term,
            };
        }
    }
}