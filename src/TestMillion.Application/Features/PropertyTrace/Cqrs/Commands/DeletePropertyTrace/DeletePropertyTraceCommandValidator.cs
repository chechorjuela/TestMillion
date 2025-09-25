using FluentValidation;

namespace TestMillion.Application.Features.PropertyTrace.Commands.DeletePropertyTrace;

public class DeletePropertyTraceCommandValidator : AbstractValidator<DeletePropertyTraceCommand>
{
    public DeletePropertyTraceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required")
            .Must(id => MongoDB.Bson.ObjectId.TryParse(id, out _))
            .WithMessage("Invalid ID format");
    }
}
