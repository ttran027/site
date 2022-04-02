using Business.Sudoku;
using Client.Components.Games.Sudoku.Store;

namespace Client.Components.Games.Sudoku;

public partial class SudokuGame
{
    private int? Pointer;

    [Inject]
    private IState<SudokuState> SudokuState { get; set; } = null!;

    [Inject]
    private IDispatcher Dispatcher { get; set; } = null!;

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            Dispatcher.Dispatch(new SudokuActions.GetPuzzle());
            SudokuState.StateChanged += HandleStateChanged;
        }
        base.OnAfterRender(firstRender);
    }

    private void HandleStateChanged(object? sender, EventArgs e)
    {
        StateHasChanged();
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

    private void SetValue(int? value)
    {
        if (Pointer is not null)
        {
            Dispatcher.Dispatch(new SudokuActions.SetValue((int)Pointer, value));         
        }
    }

    private void StartNewGame()
    {
        Dispatcher.Dispatch(new SudokuActions.NewPuzzle());
    }
}