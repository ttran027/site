namespace Client.Contract.Models.Sudoku;

public record Game(List<Group> RowGroups, List<Group> ColumnGroups, List<Group> SquareGroups);