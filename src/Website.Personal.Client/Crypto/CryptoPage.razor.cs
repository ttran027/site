namespace Website.Personal.Client.Crypto
{
    public sealed partial class CryptoPage
    {
        private string _searchString = string.Empty;

        [Inject]
        private IMediator Mediator { get; set; } = null!;

        private bool Search(CryptoInfo info)
        {
            if (!string.IsNullOrEmpty(_searchString))
            {
                return info.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ||
                       info.Ticker.Contains(_searchString, StringComparison.OrdinalIgnoreCase);
            }
            return true;
        }
    }
}