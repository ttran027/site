namespace Client.Components.Crypto.PricesTable
{
    public partial class PricesTableComponent
    {
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
        private IState<SearchState> State { get; set; }

        [Inject]
        public IDispatcher Dispatcher { get; set; }

        protected override void OnInitialized()
        {
            _searchString = State.Value.SearchTerm;
            base.OnInitialized();
        }

        private void UpdateSearchState()
        {
            Dispatcher.Dispatch(new SearchEnterAction { Term = _searchString });
        }

        private bool Search(CryptoInfo info)
        {           
            if (!string.IsNullOrEmpty(_searchString))
            {
                return info.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ||
                        info.Symbol.Contains(_searchString, StringComparison.OrdinalIgnoreCase);
            }
            return true;
        }
    }
}
