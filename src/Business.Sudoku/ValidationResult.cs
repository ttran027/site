namespace Business.Sudoku;


public record ValidationResult(bool Success, List<SudokuBlock> Blocks, string FormatError = "", bool Unsafe = false);