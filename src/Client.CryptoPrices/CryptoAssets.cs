﻿using Client.CryptoPrices.Models;

namespace Client.CryptoPrices;

public static class CryptoAssets
{
    public static IReadOnlyCollection<CryptoInfo> Assets => new List<CryptoInfo>()
    {
        new("ZRX","0x"),
        new("AAVE","Aave"),
        new("ALGO","Algorand"),
        new("AMP","Amp"),
        new("ANKR","Ankr"),
        new("REP","Augur"),
        new("AVAX","Avalanche"),
        new("AXS","Axie Infinity"),
        new("BAND","Band Protocol"),
        new("BAT","Basic Attention Token"),
        new("BTC","Bitcoin"),
        new("BCH","Bitcoin Cash"),
        new("BNB","BNB"),
        new("BUSD","BUSD"),
        new("ADA","Cardano"),
        new("CTSI","Cartesi"),
        new("LINK","ChainLink"),
        new("COMP","Compound"),
        new("ATOM","Cosmos"),
        new("CRV","Curve"),
        new("DAI","DAI"),
        new("DASH","Dash"),
        new("MANA","Decentraland"),
        new("DOGE","Dogecoin"),
        new("EGLD","Elrond"),
        new("ENJ","Enjin Coin"),
        new("EOS","EOS"),
        new("ETH","Ethereum"),
        new("ETC","Ethereum Classic"),
        new("FIL","Filecoin"),
        new("ONE","Harmony"),
        new("HBAR","Hedera Hashgraph"),
        new("HNT","Helium"),
        new("Zen","Horizen"),
        new("ICX","ICON"),
        new("KNC","Kyber Network Crystal v2"),
        new("LTC","Litecoin"),
        new("MKR","Maker"),
        new("IOTA","MIOTA"),
        new("NANO","Nano"),
        new("NEO","NEO"),
        new("OMG","OMG Network"),
        new("ONT","Ontology"),
        new("OXT","Orchid"),
        new("PAXG","Pax Gold"),
        new("DOT","Polkadot"),
        new("MATIC","Polygon"),
        new("QTUM","QTUM"),
        new("RVN","Ravencoin"),
        new("SHIB","Shiba Inu"),
        new("SOL","Solana"),
        new("XLM","Stellar Lumens"),
        new("STORJ","Storj"),
        new("SUSHI","Sushi"),
        //new("USDT","Tether"),
        new("XTZ","Tezos"),
        new("GRT","The Graph"),
        new("UNI","Uniswap"),
        new("USDC","USD Coin"),
        new("VET","Vechain"),
        new("VTHO","VeThor Token"),
        new("WAVES","Waves"),
        new("YFI","Yearn.Finance"),
        new("ZEC","Zcash"),
        new("ZIL","Zilliqa"),
    };
}