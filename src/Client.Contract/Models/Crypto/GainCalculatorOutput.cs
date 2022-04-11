namespace Client.Contract.Models.Crypto;

public class GainCalculatorOutput
{
    public decimal Invesment { get; set; }
    public decimal Gain { get; set; }
    public decimal GainRate { get; set; }
    public decimal GetFinal() => Gain + Invesment;
}