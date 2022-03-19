namespace Client.Components.Crypto.PricesTable
{
    public partial class PricesTableComponent
    {
        private MudTable<CryptoPrice> _table;
        private MudTablePager _pager;
        private string SearchString = string.Empty;
        private List<CryptoPrice> Items = new();
        private int TotalItems = 0;
        [Inject]
        private IState<PriceTableState> TableState { get; set; } = null!;

        [Inject]
        public IDispatcher Dispatcher { get; set; } = null!;

        protected override void OnAfterRender(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }

            SearchString = TableState.Value.SearchTerm;

            TableState.StateChanged += TableStateChanged;
            
            if (TableState.Value.CanGetData())
            {
                Dispatcher.Dispatch(new PriceTableActions.GetPrices());
            }
            
            base.OnAfterRender(firstRender);
        }

        private void TableStateChanged(object? sender, EventArgs e)
        {
            _table.ReloadServerData();
            InvokeAsync(StateHasChanged);
        }

        private Task<TableData<CryptoPrice>> GetItemsAsync(TableState state)
        {
            if (state.Page != TableState.Value.PageNumber)
            {
                Dispatcher.Dispatch(new PriceTableActions.ChangePageNumber(state.Page));
            }

            var data = new TableData<CryptoPrice>
            {
                Items = TableState.Value.Items,
                TotalItems = TableState.Value.Total
            };

            return Task.FromResult(data);
        }

        private void OnSearch(string searchString)
        {
            Dispatcher.Dispatch(new PriceTableActions.SearchTickers(searchString));
        }
    }
}
