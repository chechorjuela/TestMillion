using FluentValidation;
using TestMillion.Application.Features.PropertyTrace.DTOs.Request;

namespace TestMillion.Application.Features.PropertyTrace.DTOs.Request.Validators;

public class UpdatePropertyTraceRequestDtoValidator : AbstractValidator<UpdatePropertyTraceRequestDto>
{
    public UpdatePropertyTraceRequestDtoValidator()
    {
        RuleFor(x => x.DateSale)
            .NotEmpty().WithMessage("DateSale is required")
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("DateSale cannot be in the future");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Value)
            .NotEmpty().WithMessage("Value is required")
            .GreaterThan(0).WithMessage("Value must be greater than 0");

        RuleFor(x => x.Tax)
            .NotEmpty().WithMessage("Tax is required")
            .InclusiveBetween(0, 100).WithMessage("Tax must be between 0 and 100");

        RuleFor(x => x.IdProperty)
            .NotEmpty().WithMessage("Property ID is required")
            .Must(id => MongoDB.Bson.ObjectId.TryParse(id, out _))
            .WithMessage("Invalid Property ID format");
    }
}