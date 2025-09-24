using FluentValidation;
using TestMillion.Application.Common;
using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces.Base;

namespace TestMillion.Application.Properties.Commands.CreateProperty;

public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
{
  private readonly IBaseRepository<Owner> _ownerRepository;

  public CreatePropertyCommandValidator(IBaseRepository<Owner> ownerRepository)
  {
    _ownerRepository = ownerRepository;

    RuleFor(x => x.PropertyRequest.Name)
      .NotEmpty().WithMessage("Name is required")
      .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

    RuleFor(x => x.PropertyRequest.Address)
      .NotEmpty().WithMessage("Address is required")
      .MaximumLength(200).WithMessage("Address must not exceed 200 characters");

    RuleFor(x => x.PropertyRequest.Price)
      .GreaterThan(0).WithMessage("Price must be greater than 0");

    RuleFor(x => x.PropertyRequest.CodeInternal)
      .NotEmpty().WithMessage("Internal code is required")
      .MaximumLength(50).WithMessage("Internal code must not exceed 50 characters");

    RuleFor(x => x.PropertyRequest.Year)
      .GreaterThan(1800).WithMessage("Year must be greater than 1800")
      .LessThanOrEqualTo(System.DateTime.Now.Year).WithMessage("Year cannot be in the future");

    RuleFor(x => x.PropertyRequest.IdOwner)
      .NotEmpty().WithMessage("Owner ID is required")
      .MustAsync(async (id, cancellation) =>
      {
        var owner = await _ownerRepository.GetByIdAsync(id);
        return owner != null;
      }).WithMessage("Owner not found");
  }
}