namespace Client.Sudoku.Models;

internal record Game(List<Group> RowGroups, List<Group> ColumnGroups, List<Group> SquareGroups);