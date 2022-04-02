using Business.Sudoku;

namespace Client.Components.Games.Sudoku;

public partial class SudokuGame
{
    private bool IsSuccess;
    private string _errorMessage = string.Empty;
    private int? Pointer;
    private List<Business.Sudoku.Block> blocks;
    public SudokuGame()
    {
        blocks = SudokuExtensions.Generate();
    }

    private string GetBlockCssClass(Business.Sudoku.Block b)
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
     

    private void SetPointer(Business.Sudoku.Block b)
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
            var index = blocks.FindIndex(x => x.Id == Pointer && x.Hint is false);
            if (index != -1)
            {
                blocks[index] = blocks[index] with { Value = value, Invalid = false };
                Validate();
                StateHasChanged();
            }           
        }
    }

    private void Validate()
    {
        _errorMessage = string.Empty;
        var result = blocks.Validate();
        IsSuccess = result.Success;
        blocks = result.Blocks;
        _errorMessage = result.FormatError;
        StateHasChanged();
    }

    private void Restart()
    {

    }
}