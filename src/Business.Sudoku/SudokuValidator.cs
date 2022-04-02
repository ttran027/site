using FluentValidation;

namespace Business.Sudoku;

#region Game Format Validation
internal class GameValidator : AbstractValidator<Game>
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
internal class GroupValidator : AbstractValidator<Group>
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
public class BlockValidator : AbstractValidator<Block>
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