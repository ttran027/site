namespace Client.Sudoku.Models;


public record ValidationResult(bool Success, List<SudokuBlock> Blocks, string FormatError = "", bool Unsafe = false);