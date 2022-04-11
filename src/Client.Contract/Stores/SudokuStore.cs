using Client.Contract.Models.Sudoku;
using Fluxor;

namespace Client.Contract.Stores;

public record SudokuState
(
    bool IsSuccess,
    List<SudokuBlock> Blocks
);

public class SudokuFeature : Feature<SudokuState>
{
    public override string GetName() => "SudokuState";

    protected override SudokuState GetInitialState() => new(false, new List<SudokuBlock>());
}

public abstract class SudokuActions
{
    public record StateChanged();

    public record SetValue(int Id, int? Value) : StateChanged;

    public record GetPuzzle();

    public record SetPuzzle(bool Success, List<SudokuBlock> Blocks) : StateChanged;
}

public static class SudokuReducers
{
    [ReducerMethod]
    public static SudokuState OnSetPuzzle(SudokuState state, SudokuActions.SetPuzzle action)
        => state with { IsSuccess = action.Success, Blocks = action.Blocks };
}