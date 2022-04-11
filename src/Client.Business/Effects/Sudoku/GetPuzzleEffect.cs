using Client.Contract.Interfaces;
using Client.Contract.Stores;
using Fluxor;

namespace Client.Business.Effects.Sudoku;

public class GetPuzzleEffect : Effect<SudokuActions.GetPuzzle>
{
    private readonly ICacheService _cache;
    private readonly string Key = "heima.games.sudoku";
    public GetPuzzleEffect(ICacheService cache)
    {
        _cache = cache;
    }

    public override async Task HandleAsync(SudokuActions.GetPuzzle action, IDispatcher dispatcher)
    {
        var cachedState = await _cache.GetAsync<SudokuState>(Key);
        if (cachedState.IsSuccess)
        {
            dispatcher.Dispatch(new SudokuActions.SetPuzzle(cachedState.Value.IsSuccess, cachedState.Value.Blocks));
        }
    }
}