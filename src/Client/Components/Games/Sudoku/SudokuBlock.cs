namespace Client.Components.Games.Sudoku;

public record SudokuBlock(int Id, int? Value, bool Hint);

public class SudokuSolver
{
    private List<SudokuBlock> _blocks;
    public SudokuSolver(List<SudokuBlock> blocks)
    {
        _blocks = blocks;
    }
}