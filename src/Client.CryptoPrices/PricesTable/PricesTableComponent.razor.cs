using Client.Contract.Interfaces;
using Client.CryptoPrices.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Client.CryptoPrices.PricesTable
{
    public partial class PricesTableComponent
    {
        private static string TableStateKey => CryptoConstants.CacheKey("tablestate");
        private MudTable<CryptoInfo> _table;
        private string SearchString = string.Empty;

        [Inject]
        private ICacheService CacheService { get; set; } = null!;

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender) return;

            var tableState = await CacheService.GetAsync<PriceTableState>(TableStateKey);
            if (tableState.IsSuccess)
            {
                _table.CurrentPage = tableState.Value.PageNumber;
                SearchString = tableState.Value.SearchString;
            }
            
            _table.ReloadServerData();
        }

        private void OnSearch(string searchString)
        {
            _table.ReloadServerData();
        }

        private async Task<TableData<CryptoInfo>> GetItems(TableState tableState)
        {
            IEnumerable<CryptoInfo> items = CryptoAssets.Assets;
            await CacheService.SaveAsync(TableStateKey, new PriceTableState(tableState.Page, SearchString));
            if (!string.IsNullOrEmpty(SearchString))
            {
                items = items.Where(c => c.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                            c.Symbol.Contains(SearchString, StringComparison.OrdinalIgnoreCase));
            }

            return new TableData<CryptoInfo>()
            {
                Items = items.Skip(tableState.Page * 5).Take(5),
                TotalItems = items.Count(),
            };
        }
    }
}