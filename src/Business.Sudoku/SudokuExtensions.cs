using SudokuSpice;
using SudokuSpice.RuleBased;

namespace Business.Sudoku;

public static class SudokuExtensions
{
    public static List<Block> Generate()
    {
        var generator = new StandardPuzzleGenerator();
        var puzzle = generator.Generate(9, 30, TimeSpan.FromSeconds(1));
        var blocks = new List<Block>();
        foreach (var item in Enumerable.Range(0, 81))
        {
            blocks.Add(puzzle.GetBlock(item));
        }
        return blocks;
    }

    internal static int GetValue(this List<Block> board, int row, int col)
    {
        var index = board.First(x => x.RowId() == row && x.ColumnId() == col).Id;
        return board[index].Value ?? 0;
    }

    internal static void SetValue(this List<Block> board, int row, int col, int value)
    {
        var index = board.First(x => x.RowId() == row && x.ColumnId() == col).Id;
        board[index] = board[index] with { Value = value };
    }

    internal static int ToId(this Coordinate c) => (c.Row * 9) + c.Column;

    internal static Coordinate ToCoordinate(this int id)
    {
        var row = (int)decimal.Floor(id / 9);
        var col = id - (row * 9);
        return new Coordinate(row, col);
    }

    internal static Block GetBlock(this PuzzleWithPossibleValues p, int id)
    {
        var val = p[id.ToCoordinate()];
        return new Block(id, val, val is not null);
    }
   
    internal static Game GetGame(this List<Block> blocks)
    {
        var RowGroups = new List<Group>();
        for (int i = 0; i < 81; i += 9)
        {
            var rowBlocks = new List<Block>();
            for (int k = 0; k < 9; k++)
            {
                rowBlocks.Add(blocks.ElementAt(i + k));
            }
            RowGroups.Add(new(i / 9, rowBlocks));
        }

        var ColumnGroups = new List<Group>();
        for (int i = 0; i < 9; i++)
        {
            var columnBlocks = new List<Block>();
            for (int k = 0; k < 81; k += 9)
            {
                columnBlocks.Add(blocks.ElementAt(i + k));
            }
            ColumnGroups.Add(new(i, columnBlocks));
        }

        var SquareGroups = new List<Group>();
        for (int j = 0; j < 81; j += 27)
        {
            var temp1 = blocks.Skip(j);
            for (int i = 0; i < 3; i++)
            {
                var temp2 = temp1.Skip(i * 3);
                var squareBlocks = new List<Block>();
                for (int k = 0; k < 3; k++)
                {
                    squareBlocks.AddRange(temp2.Skip(k * 9).Take(3));
                }
                SquareGroups.Add(new((j / 9) + i, squareBlocks));
            }
        }
        return new(RowGroups, ColumnGroups, SquareGroups);
    }
}