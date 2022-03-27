namespace Client.Components.Games.Sudoku;

public abstract class Sudoku
{
    public record Block(int Id, int? Value, bool Hint, bool Invalid = false)
    {
        public Dictionary<int, bool> PossibleValues = new() {
                { 1, true }, { 2, true }, { 3, true }, { 4, true }, { 5, true }, { 6, true }, { 7, true }, { 8, true }, { 9, true },
            };
        public int RowId() => (int) decimal.Floor(Id/9);

        public int ColumnId() => Id - (RowId() * 9);

        public bool CanEdit() => Hint is false && PossibleValues.Where(x => x.Value is true).Count() > 1;

        public void MarkNotPossible(int value) => PossibleValues[value] = false;

        public int FirstPossible() => PossibleValues.First(x => x.Value is true).Key;
    };

    public record ValidationResult(bool Success, List<Block> Blocks, string FormatError = "", bool Unsafe = false);
    
    public record Solution(List<Block> Blocks, TimeSpan ElapsedTime);

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

    #region Sudoku Solver
    /*
     * Source code from: https://www.geeksforgeeks.org/sudoku-backtracking-7/
     * Usage: SolveSudoku(List<Block> board)
     * Return: true if success
     */
    private static bool IsSafe(List<Block> board, int row, int col, int num)
    {
        for (int d = 0; d < 9; d++)
        {
            if (board.GetValue(row, d) == num)
            {
                return false;
            }
        }

        for (int r = 0; r < 9; r++)
        {
            if (board.GetValue(r, col) == num)
            {
                return false;
            }
        }

        int sqrt = (int)Math.Sqrt(9);
        int boxRowStart = row - row % sqrt;
        int boxColStart = col - col % sqrt;

        for (int r = boxRowStart;
            r < boxRowStart + sqrt; r++)
        {
            for (int d = boxColStart;
                d < boxColStart + sqrt; d++)
            {
                if (board.GetValue(r, d) == num)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static bool SolveSudoku(List<Block> board, int n = 9)
    {
        int row = -1;
        int col = -1;
        bool isEmpty = true;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (board.GetValue(i, j) == 0)
                {
                    row = i;
                    col = j;
                    isEmpty = false;
                    break;
                }
            }
            if (!isEmpty)
            {
                break;
            }
        }

        if (isEmpty)
        {
            return true;
        }

        for (int num = 1; num <= n; num++)
        {
            if (IsSafe(board, row, col, num))
            {
                board.SetValue(row, col, num);
                if (SolveSudoku(board, n))
                {
                    return true;
                }
                else
                {
                    board.SetValue(row, col, 0);
                }
            }
        }
        return false;
    }    
    #endregion

    public static Solution Solve(List<Block> inputBlocks)
    {
        var startTime = DateTime.Now;
        SolveSudoku(inputBlocks);
        return new Solution(inputBlocks, DateTime.Now - startTime);
    }

    public record Group(int Id, List<Sudoku.Block> Blocks);

    public record Game(List<Group> RowGroups, List<Group> ColumnGroups, List<Group> SquareGroups)
    {
        public static Game GetGame(List<Sudoku.Block> blocks)
        {
            var RowGroups = new List<Group>();
            for (int i = 0; i < 81; i += 9)
            {
                var rowBlocks = new List<Sudoku.Block>();
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
                    var squareBlocks = new List<Sudoku.Block>();
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

public abstract class SudokuValidator 
{
    #region Game Format Validation
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
    #endregion

    #region Group Format Validation
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
    #endregion

    #region Block Format Validation
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
    #endregion
}