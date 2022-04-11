namespace Client.Contract.Models.Crypto;

public class GainCalculatorInput
{
    public decimal PurchasedPrice { get; set; }

    public decimal SellPrice { get; set; }

    public decimal Amount { get; set; }
}