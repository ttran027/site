using Client.Contract.Interfaces;
using Client.Sudoku.Models;
using Microsoft.AspNetCore.Components;

namespace Client.Sudoku;

public partial class SudokuGame
{
    private const string CacheKey = "heima.games.sudoku";

    private int? Pointer;

    private SudokuCache _game = new(false, new());

    [Inject] ICacheService CacheService { get;set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        var cache = await CacheService.GetAsync<SudokuCache>(CacheKey);

        if (cache.IsSuccess)
        {
            _game = cache.Value;
        }
        else
        {
            await StartNewGame();
        }
    }


    private string GetBlockCssClass(SudokuBlock b)
    {
        if (b.Hint is false)
        {
            var cssClass = "";
            if (Pointer == b.Id)
            {
                cssClass += "block-selected";
            }
            else if (b.Value is not null)
            {
                cssClass += "block-filled";
            }

            if (b.Invalid)
            {
                cssClass += " invalid";
            }
            return cssClass;
        }
        return string.Empty;
    }

    private void SetPointer(SudokuBlock b)
    {
        if (b.Hint is false)
        {
            if (Pointer == b.Id)
            {
                Pointer = null;
            }
            else
            {
                Pointer = b.Id;
            }
            StateHasChanged();
        }
    }

    private async Task SetValue(int? value)
    {
        if (Pointer is not null)
        {
            var index = _game.Blocks.FindIndex(x => x.Id == Pointer && x.Hint is false);
            if (index != -1)
            {
                _game.Blocks[index] = _game.Blocks[index] with { Value = value, Invalid = false };
            }
            var result = _game.Blocks.Validate();
            _game = new(result.Success, result.Blocks);
            await SaveToCache();
            StateHasChanged();
        }
    }

    private async Task StartNewGame()
    {
        _game = new(false, SudokuExtensions.Generate());
        await SaveToCache();
        await InvokeAsync(StateHasChanged);
    }

    private async Task SaveToCache()
    {
        await CacheService.SaveAsync(CacheKey, _game);
    }
}