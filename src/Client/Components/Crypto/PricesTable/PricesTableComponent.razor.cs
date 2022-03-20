using Client.Cache;

namespace Client.Components.Crypto.PricesTable
{
    public partial class PricesTableComponent : IDisposable
    {
        private MudTable<CryptoPrice> _table;
        private string SearchString = string.Empty;
        private List<CryptoPrice> Items = new();
        private int _pageNumber = 0;
        [Inject]
        private IState<PriceTableState> TableState { get; set; } = null!;

        [Inject]
        private IDispatcher Dispatcher { get; set; } = null!;

        [Inject]
        private IActionSubscriber ActionSubscriber { get; set; } = null!;

        [Inject]
        private ICryptoPriceCache Cache { get;set; } = null!;

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (!firstRender) return;

            if (await Cache.IsEmptyAsync())
            {
                Dispatcher.Dispatch(new PriceTableActions.GetPrices());
            }
            else
            {
                Items = (await Cache.GetPricesAsync()).ToList();
            }
            _pageNumber = TableState.Value.PageNumber;
            SearchString = TableState.Value.SearchTerm;
            await InvokeAsync(StateHasChanged);
            ActionSubscriber.SubscribeToAction<PriceTableActions.GetPricesComplete>(this, _ => OnGetPricesAction());
        }

        private async Task OnGetPricesAction()
        {
            Items = (await Cache.GetPricesAsync()).ToList();
            await InvokeAsync(StateHasChanged);
        }

        private void OnSearch(string searchString)
        {
            Dispatcher.Dispatch(new PriceTableActions.SearchTickers(searchString));
        }

        private bool Filter(CryptoPrice info)
        {
            if (!string.IsNullOrEmpty(TableState.Value.SearchTerm))
            {
                return info.Name.Contains(TableState.Value.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        info.Symbol.Contains(TableState.Value.SearchTerm, StringComparison.OrdinalIgnoreCase);
            }
            return true;
        }

        private void OnPagerClicked()
        {
            if (_table.CurrentPage != TableState.Value.PageNumber )
            {
                Dispatcher.Dispatch(new PriceTableActions.ChangePageNumber(_table.CurrentPage));
            }
            _pageNumber = _table.CurrentPage;
        }

        protected override void Dispose(bool disposing)
        {
            ActionSubscriber.UnsubscribeFromAllActions(this);
            base.Dispose(disposing);
        }
    }
}
