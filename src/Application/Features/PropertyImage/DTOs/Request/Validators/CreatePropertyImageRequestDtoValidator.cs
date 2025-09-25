using FluentValidation;
using TestMillion.Application.Features.PropertyImage.DTOs.Request;

namespace TestMillion.Application.Features.PropertyImage.DTOs.Request.Validators;

public class CreatePropertyImageRequestDtoValidator : AbstractValidator<CreatePropertyImageRequestDto>
{
    public CreatePropertyImageRequestDtoValidator()
    {
        RuleFor(x => x.File)
            .NotEmpty().WithMessage("File is required")
            .MaximumLength(1000).WithMessage("File path cannot exceed 1000 characters");

        RuleFor(x => x.IdProperty)
            .NotEmpty().WithMessage("Property ID is required")
            .Must(id => MongoDB.Bson.ObjectId.TryParse(id, out _))
            .WithMessage("Invalid Property ID format");
    }
}