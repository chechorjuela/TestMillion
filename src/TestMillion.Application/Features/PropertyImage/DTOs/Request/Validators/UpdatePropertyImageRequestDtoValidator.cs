using FluentValidation;
using TestMillion.Application.Features.PropertyImage.DTOs.Request;

namespace TestMillion.Application.Features.PropertyImage.DTOs.Request.Validators;

public class UpdatePropertyImageRequestDtoValidator : AbstractValidator<UpdatePropertyImageRequestDto>
{
    public UpdatePropertyImageRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

        RuleFor(x => x.ImagePath)
            .NotEmpty().WithMessage("Image path is required")
            .MaximumLength(1000).WithMessage("Image path cannot exceed 1000 characters");
    }
}