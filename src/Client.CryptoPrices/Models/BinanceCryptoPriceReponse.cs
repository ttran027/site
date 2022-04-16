namespace Client.CryptoPrices.Models;

internal record BinanceCryptoPriceResponse
(
    string LastPrice,
    string PriceChangePercent
);