using FluentValidation;

namespace TestMillion.Application.Features.Properties.Cqrs.Commands.DeleteProperty;

public class DeletePropertyCommandValidator : AbstractValidator<DeletePropertyCommand>
{
    public DeletePropertyCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required")
            .Must(id => MongoDB.Bson.ObjectId.TryParse(id, out _))
            .WithMessage("Invalid ID format");
    }
}
