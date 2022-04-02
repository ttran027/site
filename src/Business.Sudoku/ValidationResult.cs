namespace Business.Sudoku;


public record ValidationResult(bool Success, List<Block> Blocks, string FormatError = "", bool Unsafe = false);