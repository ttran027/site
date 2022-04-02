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
    public record SetValue(int Id, int? Value);

    public record SetPuzzle(List<SudokuBlock> Blocks);
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
    public static SudokuState OnSetPuzzke(SudokuState state, SudokuActions.SetPuzzle actions)
        => state with { IsSuccess = false, Blocks = actions.Blocks };
}