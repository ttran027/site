namespace Client.Components.Games.Sudoku;

public abstract class Sudoku
{
    public record Block(int Id, int? Value, bool Hint);

    public record Group(int Id, List<Block> Blocks);

    public class Solver
    {
        private List<Block> _blocks;
        public List<Group> RowGroups { get; init; }
        public List<Group> ColumnGroups { get; init; }
        public List<Group> SquareGroups { get; init; }

        public Solver(List<Block> blocks)
        {
            _blocks = blocks;
            RowGroups = new List<Group>();
            for (int i = 0; i < 81; i += 9)
            {
                var rowBlocks = new List<Block>();
                for (int k = 0; k < 9; k++)
                {
                    rowBlocks.Add(blocks.ElementAt(i + k));
                }
                RowGroups.Add(new(i / 9, rowBlocks));
            }

            ColumnGroups = new List<Group>();
            for (int i = 0; i < 9; i++)
            {
                var columnBlocks = new List<Block>();
                for (int k = 0; k < 81; k += 9)
                {
                    columnBlocks.Add(blocks.ElementAt(i + k));
                }
                ColumnGroups.Add(new(i, columnBlocks));
            }

            SquareGroups = new List<Group>();
            for (int j = 0; j < 81; j += 27)
            {
                var temp1 = _blocks.Skip(j);
                for (int i = 0; i < 3; i++)
                {
                    var temp2 = temp1.Skip(i * 3);
                    var squareBlocks = new List<Block>();
                    for (int k = 0; k < 3; k++)
                    {
                        squareBlocks.AddRange(temp2.Skip(k * 9).Take(3));
                    }
                    SquareGroups.Add(new((j/9) + i, squareBlocks));
                }
            }
        }
    }
}