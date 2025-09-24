using FluentValidation;

namespace TestMillion.Application.Features.Owners.Commands.DeleteOwner;

public class DeleteOwnerCommandValidator : AbstractValidator<DeleteOwnerCommand>
{
    public DeleteOwnerCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
    }
}