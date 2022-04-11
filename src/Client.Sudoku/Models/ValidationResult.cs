namespace Client.Sudoku.Models;

internal record ValidationResult(bool Success, List<SudokuBlock> Blocks, string FormatError = "", bool Unsafe = false);