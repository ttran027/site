using Business.Sudoku;

namespace Client.Components.Games.Sudoku.Store;

public record SudokuState
(
    bool IsSuccess,
    List<SudokuBlock> Blocks
);

public class SudokuFeature : Feature<SudokuState>
{
    public override string GetName() => "SudokuState";

    protected override SudokuState GetInitialState() => new(false, SudokuExtensions.Generate());  
}

public abstract class SudokuActions
{
    public record StateChanged();

    public record SetValue(int Id, int? Value) : StateChanged;

    public record NewPuzzle() : StateChanged;

    public record GetPuzzle();

    public record SetPuzzle(bool Success, List<SudokuBlock> Blocks) : StateChanged;
}

public static class SudokuReducers
{
    [ReducerMethod]
    public static SudokuState OnSetValue(SudokuState state, SudokuActions.SetValue actions) 
    {
        var index = state.Blocks.FindIndex(x => x.Id == actions.Id && x.Hint is false);
        if (index != -1)
        {
            state.Blocks[index] = state.Blocks[index] with { Value = actions.Value, Invalid = false };
        }
        var result = state.Blocks.Validate();
        return state with
        {
            IsSuccess = result.Success,
            Blocks = result.Blocks,
        };
    }

    [ReducerMethod]
    public static SudokuState OnNewPuzzle(SudokuState state, SudokuActions.NewPuzzle _)
        => state with { IsSuccess = false, Blocks = SudokuExtensions.Generate() };

    [ReducerMethod]
    public static SudokuState OnSetPuzzle(SudokuState state, SudokuActions.SetPuzzle action)
        => state with { IsSuccess = action.Success, Blocks = action.Blocks };
}