namespace Client.CryptoPrices.Models;

public class GainCalculatorInput
{
    public decimal PurchasedPrice { get; set; }

    public decimal SellPrice { get; set; }

    public decimal Amount { get; set; }
}