using Client.Cache;

namespace Client.Components.Games.Sudoku.Store.Effects;

public class GetPuzzleEffect : Effect<SudokuActions.GetPuzzle>
{
    private readonly ISudokuCache _cache;

    public GetPuzzleEffect(ISudokuCache cache)
    {
        _cache = cache;
    }

    public override async Task HandleAsync(SudokuActions.GetPuzzle action, IDispatcher dispatcher)
    {
        var cachedState = await _cache.GetStateAsync();
        if (cachedState == null)
        {
            dispatcher.Dispatch(new SudokuActions.NewPuzzle());
        }
        else
        {
            dispatcher.Dispatch(new SudokuActions.SetPuzzle(cachedState.IsSuccess, cachedState.Blocks));
        }
    }
}