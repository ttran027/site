namespace Client.Components.Games.Sudoku;

public partial class SudokuGame
{
    private bool IsSuccess;
    private string _errorMessage = string.Empty;
    private int? Pointer;
    private List<Sudoku.Block> blocks;
    public SudokuGame()
    {
        var test = ",,,,,6,1,8,,,8,,,7,,3,9,,,3,,4,,,7,,,,,,5,,,8,,,9,,,,,,,,1,,,7,,,1,,,,,,9,,,3,,5,,,7,5,,6,,,3,,,2,6,9,,,,,";
        var list = test.Split(',');
        blocks = list
            .Select((e,i) => new Sudoku.Block(i, string.IsNullOrEmpty(e) ? null : int.Parse(e), !string.IsNullOrEmpty(e)))
            .ToList();
    }

    private string GetBlockCssClass(Sudoku.Block b)
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
     

    private void SetPointer(Sudoku.Block b)
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
        var result = Sudoku.Validate(blocks);
        IsSuccess = result.Success;
        blocks = result.Blocks;
        _errorMessage = result.FormatError;
        StateHasChanged();
    }

    private void Restart()
    {

    }
}