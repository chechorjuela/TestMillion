using FluentValidation;

namespace TestMillion.Application.Features.PropertyImage.Commands.DeletePropertyImage;

public class DeletePropertyImageCommandValidator : AbstractValidator<DeletePropertyImageCommand>
{
    public DeletePropertyImageCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required")
            .Must(id => MongoDB.Bson.ObjectId.TryParse(id, out _))
            .WithMessage("Invalid ID format");
    }
}
