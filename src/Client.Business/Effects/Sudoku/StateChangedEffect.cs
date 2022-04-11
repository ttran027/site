using Client.Contract.Interfaces;
using Client.Contract.Stores;
using Fluxor;

namespace Client.Business.Effects.Sudoku;

public class StateChangedEffect : Effect<SudokuActions.StateChanged>
{
    private readonly ICacheService _cache;
    private readonly IState<SudokuState> _state;
    private readonly string Key = "heima.games.sudoku";

    public StateChangedEffect(ICacheService cache, IState<SudokuState> state)
    {
        _cache = cache;
        _state = state;
    }

    public override async Task HandleAsync(SudokuActions.StateChanged action, IDispatcher dispatcher)
    {
        await _cache.SaveAsync(Key, _state.Value);
    }
}