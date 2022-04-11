using Client.Contract.Models.Crypto;
using Fluxor;

namespace Client.Contract.Stores;

public record GainCalculatorState
(
    GainCalculatorInput Input,
    GainCalculatorOutput Output
);

public class GainCalculatorFeature : Feature<GainCalculatorState>
{
    public override string GetName() => "GainCalculator";

    protected override GainCalculatorState GetInitialState()
     => new(
            new GainCalculatorInput() { Amount = 1000 },
            new GainCalculatorOutput());
}

public class GainCalculatorInputsEnterAction
{
    public GainCalculatorInput Input { get; set; }
    public GainCalculatorOutput Output { get; set; }
}

public static class GainCalculatorReducers
{
    [ReducerMethod]
    public static GainCalculatorState OnEnter(GainCalculatorState state, GainCalculatorInputsEnterAction action)
        => state with
        {
            Input = action.Input,
            Output = action.Output
        };
}
