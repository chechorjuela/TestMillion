using FluentValidation;

namespace TestMillion.Application.Features.Owners.DTOs.Request.Validators;

public class UpdateOwnerRequestDtoValidator : AbstractValidator<UpdateOwnerRequestDto>
{
    public UpdateOwnerRequestDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(200).WithMessage("Address must not exceed 200 characters");

        RuleFor(x => x.Photo)
            .NotEmpty().WithMessage("Photo URL is required")
            .Must(BeAValidUrl).WithMessage("Photo must be a valid URL");

        RuleFor(x => x.Birthdate)
            .NotEmpty().WithMessage("Birthdate is required")
            .Must(BeAValidBirthdate).WithMessage("Birthdate must be in the past");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }

    private bool BeAValidBirthdate(DateOnly date)
    {
        return date < DateOnly.FromDateTime(DateTime.UtcNow);
    }
}