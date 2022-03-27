using Client.Components.Games.Sudoku;

internal static class SudokuSolverExtensions
{
    internal static int GetValue(this List<Sudoku.Block> board, int row, int col)
    {
        var index = board.First(x => x.RowId() == row && x.ColumnId() == col).Id;
        return board[index].Value ?? 0;
    }

    internal static void SetValue(this List<Sudoku.Block> board, int row, int col, int value)
    {
        var index = board.First(x => x.RowId() == row && x.ColumnId() == col).Id;
        board[index] = board[index] with { Value = value };
    }
}