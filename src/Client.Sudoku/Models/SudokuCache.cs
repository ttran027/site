namespace Client.Sudoku.Models;

internal record SudokuCache
(
    bool IsSuccess,
    List<SudokuBlock> Blocks
);