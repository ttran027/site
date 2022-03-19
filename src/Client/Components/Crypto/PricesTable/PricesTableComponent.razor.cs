namespace Client.Components.Crypto.PricesTable
{
    public partial class PricesTableComponent
    {
        private MudTable<CryptoInfo> _table;
        private string _searchString = string.Empty;
        /// <summary>
        /// A function that takes in symbol name as <see cref="string"/> and returns <seealso cref="CryptoPrice"/>.
        /// This function must be passed to display the price.
        /// </summary>
        [Parameter]
        [EditorRequired]
        public Func<string, Task<CryptoPrice>> GetPrice { get; set; }

        /// <summary>
        /// A list of crypto assets to be supported int this table
        /// </summary>
        [Parameter]
        public ICollection<CryptoInfo> Assets { get; set; }

        [Inject]
        private IState<PriceTableState> TableState { get; set; } = null!;

        [Inject]
        public IDispatcher Dispatcher { get; set; } = null!;

        protected override void OnInitialized()
        {
            _searchString = TableState.Value.SearchTerm;
            base.OnInitialized();
        }

        private Task<TableData<CryptoInfo>> GetItemsAsync(TableState state)
        {
            if (state.Page != TableState.Value.PageNumber)
            {
                Dispatcher.Dispatch(new PriceTableActions.ChangePageNumber(state.Page));
            }

            var items = Assets.Where(IsMatched).ToList();
            var filterItems = items.Skip(state.PageSize * state.Page).Take(state.PageSize);

            var data = new TableData<CryptoInfo>
            {
                Items = filterItems,
                TotalItems = items.Count,
            };
            return Task.FromResult(data);
        }

        private void OnSearch(string searchString)
        {
            Dispatcher.Dispatch(new PriceTableActions.SearchTickers(searchString));
            _table.ReloadServerData();
        }

        private bool IsMatched(CryptoInfo info)
        {           
            if (!string.IsNullOrEmpty(TableState.Value.SearchTerm))
            {
                return info.Name.Contains(TableState.Value.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        info.Symbol.Contains(TableState.Value.SearchTerm, StringComparison.OrdinalIgnoreCase);
            }
            return true;
        }
    }
}
