using Blazored.LocalStorage;
using Client.Sudoku.Store;

namespace Client.Sudoku;

public interface ISudokuCache
{
    Task SaveStateAsync(SudokuState state);

    Task<SudokuState?> GetStateAsync();
}

public class SudokuCache : ISudokuCache
{
    private readonly ILocalStorageService _localStorage;
    private readonly string Key = "heima.games.sudoku";
    public SudokuCache(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<SudokuState?> GetStateAsync()
    {
        if (await _localStorage.ContainKeyAsync(Key))
        {
            return await _localStorage.GetItemAsync<SudokuState>(Key);
        }
        return null;
    }

    public async Task SaveStateAsync(SudokuState state)
    {
        await _localStorage.SetItemAsync(Key, state);
    }
}