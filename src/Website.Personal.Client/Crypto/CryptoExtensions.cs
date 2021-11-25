namespace Website.Personal.Client.Crypto
{
    public static class CryptoExtensions 
    {
        private static readonly List<string> List = new() { "0x-ZRX", "1inch-1INCH", "Aave-AAVE", "Alchemy Pay-ACH", "Adventure Gold-AGLD", "Algorand-ALGO", "Amp-AMP", "Ampleforth Governance Token-FORTH", "Ankr-ANKR", "Augur-REP", "Avalanche-AVAX", "Axie Infinity-AXS", "Balancer-BAL", "Bancor Network Token-BNT", "Band Protocol-BAND", "BarnBridge-BOND", "Basic Attention Token-BAT", "Bitcoin-BTC", "Bitcoin Cash-BCH", "Braintrust-BTRST", "Cardano-ADA", "Cartesi-CTSI", "Celo-CGLD", "Chainlink-LINK", "Chiliz-CHZ", "Civic-CVC", "Clover Finance-CLV", "Compound-COMP", "Cosmos-ATOM", "COTI-COTI", "Curve DAO Token-CRV", "Dai-DAI", "Dash-DASH", "Decentraland-MANA", "DerivaDAO-DDX", "DFI.Money-YFII", "District0x-DNT", "Dogecoin-DOGE", "Enjin Coin-ENJ", "Enzyme-MLN", "EOS-EOS", "Ethereum-ETH", "Ethereum Classic-ETC", "Fetch.ai-FET", "Filecoin-FIL", "Function X-FX", "Gitcoin-GTC", "Golem-GNT", "Harvest Finance-FARM", "Horizen-ZEN", "iExec RLC-RLC", "Internet Computer-ICP", "IoTeX-IOTX", "Jasmy-JASMY", "Keep Network-KEEP", "Kyber Network-KNC", "Litecoin-LTC", "Livepeer (LPT)-LPT", "Loom Network-LOOM", "Loopring-LRC", "Maker-MKR", "Mask Network-MASK", "Mirror Protocol-MIR", "NKN-NKN", "NuCypher-NU", "Numeraire-NMR", "OMG Network-OMG", "Orchid-OXT", "Origin Token-OGN", "Orion Protocol-ORN", "Paxos Standard-PAX", "PlayDapp-PLA", "Polkadot-DOT", "Polygon-MATIC", "Polymath-POLY", "Quant-QNT", "QuickSwap-QUICK", "Radicle-RAD", "Rai Reflex Index-RAI", "Rally-RLY", "Rari Governance Token-RGT", "Ren-REN", "Request-REQ", "Shiba Inu-SHIB", "SKALE-SKL", "Solana-SOL", "Stellar Lumens-XLM", "STORJ-STORJ", "SushiSwap-SUSHI", "Synthetix Network Token-SNX", "tBTC-TBTC", "Tellor-TRB", "TerraUSD-UST", "Tether-USDT", "Tezos-XTZ", "The Graph-GRT", "Tribe-TRIBE", "TrueFi-TRU", "UMA-UMA", "Uniswap-UNI", "USD Coin-USDC", "Wrapped Bitcoin-WBTC", "Wrapped Centrifuge-wCFG", "Wrapped LUNA-WLUNA", "XYO Network-XYO", "yearn.finance-YFI", "Zcash-ZEC" };
        
        private static CryptoInfo ToCryptoInfo(this string c)
        {
            var terms = c.Split('-');
            return new CryptoInfo(terms[1], terms[0]);
        }

        public static List<CryptoInfo> SupportedCrypto => List.Select(e => e.ToCryptoInfo()).ToList();

        public static RenderFragment ToPercent(this string p) => builder =>
        {
            builder.OpenElement(0, "span");
            var isNegative = p.StartsWith("-");
            var color = isNegative ? "red-text" : "green-text";
            builder.AddAttribute(1, "class", color);
            builder.AddContent(2, (isNegative ? p : $"+{p}") + "%");
            builder.CloseElement();
        };
    }
}