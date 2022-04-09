using FluentValidation;

namespace Client.CryptoPrices.GainCalculator
{
    public class GainCalculatorInputValidator : AbstractValidator<GainCalculatorInput>
    {
        public GainCalculatorInputValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than 0");

            RuleFor(x => x.PurchasedPrice)
                .GreaterThan(0)
                .WithMessage("Purchased price must be greater than 0");

            RuleFor(x => x.SellPrice)
                .GreaterThan(0)
                .WithMessage("Sell price must be greater than 0");
        }
    }
}