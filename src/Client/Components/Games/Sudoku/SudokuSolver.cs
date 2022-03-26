namespace Client.Components.Games.Sudoku;

public abstract class Sudoku
{
    public record Block(int Id, int? Value, bool Hint, bool Invalid = false)
    {
        public int RowId() => Id % 9;

        public int ColumnId() => Id - (RowId() * 9);
    };

    public record ValidationResult(bool Success, List<Block> Blocks, string FormatError = "");
    
    public static ValidationResult Validate(List<Block> inputBlocks)
    {
        var blocks = new List<Block>();
        for (int i = 0; i < inputBlocks.Count; i++)
        {
            blocks.Add(inputBlocks[i] with { Invalid = false });
        }
        var game = Game.GetGame(blocks);
        var validator = new SudokuValidator.GameValidator();
        var result = validator.Validate(game);
        if (!result.IsValid)
        {
            return new ValidationResult(false, blocks, FormatError: result.ToString());
        }

        var invalidBlocks = GetInvalidGameBlocks(game);
        if (!invalidBlocks.Any())
        {
            return new ValidationResult(true, blocks);
        }

        foreach (var block in invalidBlocks)
        {
            blocks[block] = blocks[block] with { Invalid = true };
        }

        return new ValidationResult(false, blocks);
    }

    internal record Group(int Id, List<Block> Blocks)
    {
        public bool IsValid(int number) => !Blocks.Any(x => x.Value == number);
    };

    internal record Game(List<Group> RowGroups, List<Group> ColumnGroups, List<Group> SquareGroups)
    {
        public static Game GetGame(List<Block> blocks)
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
    };
    
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

internal abstract class SudokuValidator 
{
    public class GameValidator : AbstractValidator<Sudoku.Game>
    {
        public GameValidator()
        {
            RuleForEach(x => x.RowGroups)
                .SetValidator(new GroupValidator());

            RuleForEach(x => x.ColumnGroups)
                .SetValidator(new GroupValidator());

            RuleForEach(x => x.SquareGroups)
                .SetValidator(new GroupValidator());
        }
    }

    public class GroupValidator : AbstractValidator<Sudoku.Group>
    {
        public GroupValidator()
        {
            RuleForEach(x => x.Blocks)
                .SetValidator(new BlockValidator());

            RuleFor(x => x.Blocks)
                .Must(x => x.Count is 9)
                .WithMessage("Group must have 9 blocks");

            RuleFor(x => x.Id)
                .InclusiveBetween(0, 9)
                .WithName("Group ID");
        }
    }

    public class BlockValidator : AbstractValidator<Sudoku.Block>
    {
        public BlockValidator()
        {
            RuleFor(x => x.Id)
                .InclusiveBetween(0, 80)
                .WithName("Block ID");

            RuleFor(x => x.Value)
                .InclusiveBetween(0, 9)
                .When(x => x.Value is not null)
                .WithName("Block Value");
        }
    }
}