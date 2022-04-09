using Fluxor;

namespace Client.Sudoku.Store.Effects;

public class StateChangedEffect : Effect<SudokuActions.StateChanged>
{
    private readonly ISudokuCache _cache;
    private readonly IState<SudokuState> _state;
    public StateChangedEffect(ISudokuCache cache, IState<SudokuState> state)
    {
        _cache = cache;
        _state = state;
    }

    public override async Task HandleAsync(SudokuActions.StateChanged action, IDispatcher dispatcher)
    {
        await _cache.SaveStateAsync(_state.Value);
    }
}