using FluentValidation;

namespace Client.Sudoku.Models;

public static class SudokuValidatorExtensions
{
    public static ValidationResult Validate(this List<SudokuBlock> inputBlocks)
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
}