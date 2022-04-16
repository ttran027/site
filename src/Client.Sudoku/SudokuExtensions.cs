using Client.Sudoku.Models;
using SudokuSpice;
using SudokuSpice.RuleBased;

namespace Client.Sudoku;

internal static class SudokuExtensions
{
    internal static List<SudokuBlock> Generate()
    {
        var generator = new StandardPuzzleGenerator();
        var puzzle = generator.Generate(9, 30, TimeSpan.FromSeconds(1));
        var blocks = new List<SudokuBlock>();
        foreach (var item in Enumerable.Range(0, 81))
        {
            blocks.Add(puzzle.GetBlock(item));
        }
        return blocks;
    }

    internal static int GetValue(this List<SudokuBlock> board, int row, int col)
    {
        var index = board.First(x => x.RowId() == row && x.ColumnId() == col).Id;
        return board[index].Value ?? 0;
    }

    internal static void SetValue(this List<SudokuBlock> board, int row, int col, int value)
    {
        var index = board.First(x => x.RowId() == row && x.ColumnId() == col).Id;
        board[index] = board[index] with { Value = value };
    }

    internal static int ToId(this Coordinate c) => c.Row * 9 + c.Column;

    internal static Coordinate ToCoordinate(this int id)
    {
        var row = (int)decimal.Floor(id / 9);
        var col = id - row * 9;
        return new Coordinate(row, col);
    }

    internal static SudokuBlock GetBlock(this PuzzleWithPossibleValues p, int id)
    {
        var val = p[id.ToCoordinate()];
        return new SudokuBlock(id, val, val is not null);
    }
}