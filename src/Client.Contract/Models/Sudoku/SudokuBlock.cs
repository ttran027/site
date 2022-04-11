namespace Client.Contract.Models.Sudoku;

public record SudokuBlock(int Id, int? Value, bool Hint, bool Invalid = false)
{
    public Dictionary<int, bool> PossibleValues = new()
    {
        { 1, true },
        { 2, true },
        { 3, true },
        { 4, true },
        { 5, true },
        { 6, true },
        { 7, true },
        { 8, true },
        { 9, true },
    };
    public int RowId() => (int)decimal.Floor(Id / 9);

    public int ColumnId() => Id - RowId() * 9;

    public bool CanEdit() => Hint is false && PossibleValues.Where(x => x.Value is true).Count() > 1;

    public void MarkNotPossible(int value) => PossibleValues[value] = false;

    public int FirstPossible() => PossibleValues.First(x => x.Value is true).Key;
};