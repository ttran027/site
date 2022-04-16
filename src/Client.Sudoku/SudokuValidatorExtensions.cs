using Client.Sudoku.Models;
using FluentValidation;

namespace Client.Sudoku;

internal static class SudokuValidatorExtensions
{
    internal static ValidationResult Validate(this List<SudokuBlock> inputBlocks)
    {
        var blocks = new List<SudokuBlock>();
        for (int i = 0; i < inputBlocks.Count; i++)
        {
            blocks.Add(inputBlocks[i] with { Invalid = false });
        }
        var game = blocks.GetGame();
        var validator = new GameValidator();
        var result = validator.Validate(game);
        if (!result.IsValid)
        {
            return new ValidationResult(false, blocks, FormatError: result.ToString());
        }

        var invalidBlocks = GetInvalidGameBlocks(game);

        if (invalidBlocks.Any())
        {
            foreach (var block in invalidBlocks)
            {
                blocks[block] = blocks[block] with { Invalid = true };
            }
            return new ValidationResult(false, blocks, Unsafe: true);
        }
        return new ValidationResult(!blocks.Any(x => x.Value is null), blocks);
    }

    private static IEnumerable<int> GetInvalidGameBlocks(Game game)
    {
        var list = new List<int>();
        foreach (var item in game.ColumnGroups)
        {
            list.AddRange(GetInvalidBlockIds(item));
        }
        foreach (var item in game.RowGroups)
        {
            list.AddRange(GetInvalidBlockIds(item));
        }
        foreach (var item in game.SquareGroups)
        {
            list.AddRange(GetInvalidBlockIds(item));
        }
        return list.Distinct();
    }

    private static IEnumerable<int> GetInvalidBlockIds(Group group)
    {
        var invalidBlocksIds = group.Blocks.Where(b => b.Value is not null)
                     .GroupBy(b => b.Value,
                              b => b,
                              (key, g) => new { Value = key, Count = g.Count(), Ids = g.Select(ge => ge.Id) })
                     .Where(e => e.Count > 1)
                     .Select(e => e.Ids);
        var list = new List<int>();
        foreach (var l in invalidBlocksIds)
        {
            list.AddRange(l);
        }
        return list.Distinct();
    }

    private static Game GetGame(this List<SudokuBlock> blocks)
    {
        var RowGroups = new List<Group>();
        for (int i = 0; i < 81; i += 9)
        {
            var rowBlocks = new List<SudokuBlock>();
            for (int k = 0; k < 9; k++)
            {
                rowBlocks.Add(blocks.ElementAt(i + k));
            }
            RowGroups.Add(new(i / 9, rowBlocks));
        }

        var ColumnGroups = new List<Group>();
        for (int i = 0; i < 9; i++)
        {
            var columnBlocks = new List<SudokuBlock>();
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
                var squareBlocks = new List<SudokuBlock>();
                for (int k = 0; k < 3; k++)
                {
                    squareBlocks.AddRange(temp2.Skip(k * 9).Take(3));
                }
                SquareGroups.Add(new(j / 9 + i, squareBlocks));
            }
        }
        return new(RowGroups, ColumnGroups, SquareGroups);
    }
}