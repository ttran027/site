namespace Client.Sudoku.Models;

internal record SudokuBlock(int Id, int? Value, bool Hint, bool Invalid = false)
{
    internal Dictionary<int, bool> PossibleValues = new()
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
    internal int RowId() => (int)decimal.Floor(Id / 9);

    internal int ColumnId() => Id - RowId() * 9;

    internal bool CanEdit() => Hint is false && PossibleValues.Where(x => x.Value is true).Count() > 1;

    internal void MarkNotPossible(int value) => PossibleValues[value] = false;

    internal int FirstPossible() => PossibleValues.First(x => x.Value is true).Key;
};