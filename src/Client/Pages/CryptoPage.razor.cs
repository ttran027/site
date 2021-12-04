using Client.Components.Crypto.PricesTable;
using Client.CQRS.Queries;

namespace Client.Pages
{
    public sealed partial class CryptoPage
    {
        [Inject]
        private IMediator Mediator { get; set; } = null!;

        private readonly List<string> Assets = new() { "0x-ZRX", "1inch-1INCH", "Aave-AAVE", "Alchemy Pay-ACH", "Adventure Gold-AGLD", "Algorand-ALGO", "Amp-AMP", "Ampleforth Governance Token-FORTH", "Ankr-ANKR", "Augur-REP", "Avalanche-AVAX", "Axie Infinity-AXS", "Balancer-BAL", "Bancor Network Token-BNT", "Band Protocol-BAND", "BarnBridge-BOND", "Basic Attention Token-BAT", "Bitcoin-BTC", "Bitcoin Cash-BCH", "Braintrust-BTRST", "Cardano-ADA", "Cartesi-CTSI", "Celo-CGLD", "Chainlink-LINK", "Chiliz-CHZ", "Civic-CVC", "Clover Finance-CLV", "Compound-COMP", "Cosmos-ATOM", "COTI-COTI", "Curve DAO Token-CRV", "Dai-DAI", "Dash-DASH", "Decentraland-MANA", "DerivaDAO-DDX", "DFI.Money-YFII", "District0x-DNT", "Dogecoin-DOGE", "Enjin Coin-ENJ", "Enzyme-MLN", "EOS-EOS", "Ethereum-ETH", "Ethereum Classic-ETC", "Fetch.ai-FET", "Filecoin-FIL", "Function X-FX", "Gitcoin-GTC", "Golem-GNT", "Harvest Finance-FARM", "Horizen-ZEN", "iExec RLC-RLC", "Internet Computer-ICP", "IoTeX-IOTX", "Jasmy-JASMY", "Keep Network-KEEP", "Kyber Network-KNC", "Litecoin-LTC", "Livepeer (LPT)-LPT", "Loom Network-LOOM", "Loopring-LRC", "Maker-MKR", "Mask Network-MASK", "Mirror Protocol-MIR", "NKN-NKN", "NuCypher-NU", "Numeraire-NMR", "OMG Network-OMG", "Orchid-OXT", "Origin Token-OGN", "Orion Protocol-ORN", "Paxos Standard-PAX", "PlayDapp-PLA", "Polkadot-DOT", "Polygon-MATIC", "Polymath-POLY", "Quant-QNT", "QuickSwap-QUICK", "Radicle-RAD", "Rai Reflex Index-RAI", "Rally-RLY", "Rari Governance Token-RGT", "Ren-REN", "Request-REQ", "Shiba Inu-SHIB", "SKALE-SKL", "Solana-SOL", "Stellar Lumens-XLM", "STORJ-STORJ", "SushiSwap-SUSHI", "Synthetix Network Token-SNX", "tBTC-TBTC", "Tellor-TRB", "TerraUSD-UST", "Tether-USDT", "Tezos-XTZ", "The Graph-GRT", "Tribe-TRIBE", "TrueFi-TRU", "UMA-UMA", "Uniswap-UNI", "USD Coin-USDC", "Wrapped Bitcoin-WBTC", "Wrapped Centrifuge-wCFG", "Wrapped LUNA-WLUNA", "XYO Network-XYO", "yearn.finance-YFI", "Zcash-ZEC" };

        private readonly string PanelStyle = "display: flex; flex-direction: column; align-items: center; width: 460px";

        private readonly string Base = "USDT";
        private static RenderFragment GetTitle(string titleText, string href) => builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "d-flex flex-row align-end");
            builder.OpenComponent<IconLink>(2);
            builder.AddAttribute(3, "Icon", Icons.Custom.Brands.GitHub);
            builder.AddAttribute(4, "Size", Size.Small);
            builder.AddAttribute(5, "Href", href);
            builder.AddAttribute(6, "TooltipText", "Click for source code");
            builder.AddAttribute(7, "Style", "color: #ff9800 !important");
            builder.AddAttribute(8, "Class", "mr-2");
            builder.AddAttribute(9, "NewTab", true);
            builder.CloseComponent();
            builder.OpenComponent<MudText>(10);
            builder.AddAttribute(11, "Color", Color.Secondary);
            builder.AddAttribute(12, "Typo", Typo.h6);
            builder.AddAttribute(13, "ChildContent", (RenderFragment)(builder2 =>
            {
                builder2.AddContent(14, titleText);
            }));
            builder.CloseComponent();
            builder.CloseElement();
        };

        private static CryptoInfo GetCryptoInfo(string c)
        {
            var terms = c.Split('-');
            return new CryptoInfo(terms[1], terms[0]);
        }

        private List<CryptoInfo> SupportedCrypto => Assets.Select(e => GetCryptoInfo(e)).ToList();

        private async Task<CryptoPrice> GetPriceAsync(string symbol)
        {
           
            var result = await Mediator.Send(new CryptoPriceQuery() { Symbol = symbol, Base=Base });
            if (result.IsSuccess)
            {
                return new CryptoPrice(symbol, Base, double.Parse(result.Value.LastPrice), double.Parse(result.Value.PriceChangePercent));
            }
            return new CryptoPrice(symbol, Base, null, null);
        }
    }
}