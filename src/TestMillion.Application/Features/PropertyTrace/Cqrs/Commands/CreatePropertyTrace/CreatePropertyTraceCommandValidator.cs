using FluentValidation;

namespace TestMillion.Application.Features.PropertyTrace.Commands.CreatePropertyTrace;

public class CreatePropertyTraceCommandValidator : AbstractValidator<CreatePropertyTraceCommand>
{
    public CreatePropertyTraceCommandValidator()
    {
        RuleFor(x => x.DateSale)
            .NotEmpty().WithMessage("Date of sale is required")
            .Must(date => date <= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Date of sale cannot be in the future");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Value)
            .GreaterThan(0).WithMessage("Value must be greater than 0")
            .LessThan(decimal.MaxValue).WithMessage("Value is too large");

        RuleFor(x => x.Tax)
            .InclusiveBetween(0, 100).WithMessage("Tax must be between 0 and 100");

        RuleFor(x => x.IdProperty)
            .NotEmpty().WithMessage("Property ID is required")
            .Must(id => MongoDB.Bson.ObjectId.TryParse(id, out _))
            .WithMessage("Invalid Property ID format");
    }
}
