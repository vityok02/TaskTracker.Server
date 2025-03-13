using FluentValidation;

namespace Application.Modules.Members.AddMember;

public class AddMemberCommandValidator
    : AbstractValidator<AddMemberCommand>
{
    public AddMemberCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty)
            .NotNull();

        RuleFor(x => x.ProjectId)
            .NotEqual(Guid.Empty)
            .NotNull();

        RuleFor(x => x.RoleId)
            .NotEqual(Guid.Empty)
            .NotNull();
    }
}
