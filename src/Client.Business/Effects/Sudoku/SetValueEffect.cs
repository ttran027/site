using Client.Contract.Models.Sudoku;
using Client.Contract.Stores;
using Fluxor;

namespace Client.Business.Effects.Sudoku;

public class SetValueEffect : Effect<SudokuActions.SetValue>
{
    private readonly IState<SudokuState> _state;

    public SetValueEffect(IState<SudokuState> state)
    {
        _state = state;
    }

    public override async Task HandleAsync(SudokuActions.SetValue action, IDispatcher dispatcher)
    {
        var index = _state.Value.Blocks.FindIndex(x => x.Id == action.Id && x.Hint is false);
        if (index != -1)
        {
            _state.Value.Blocks[index] = _state.Value.Blocks[index] with { Value = action.Value, Invalid = false };
        }
        var result = _state.Value.Blocks.Validate();
        dispatcher.Dispatch(new SudokuActions.SetPuzzle(result.Success, result.Blocks));
    }
}