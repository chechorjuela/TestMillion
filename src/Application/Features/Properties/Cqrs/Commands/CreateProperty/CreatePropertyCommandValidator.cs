using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces.Base;

namespace TestMillion.Application.Features.Properties.Cqrs.Commands.CreateProperty;

public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
{
  private readonly IBaseRepository<Owner> _ownerRepository;

  public CreatePropertyCommandValidator(IBaseRepository<Owner> ownerRepository)
  {
    _ownerRepository = ownerRepository;

    RuleFor(x => x.Name)
      .NotEmpty().WithMessage("Name is required")
      .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

    RuleFor(x => x.Address)
      .NotEmpty().WithMessage("Address is required")
      .MaximumLength(200).WithMessage("Address must not exceed 200 characters");

    RuleFor(x => x.Price)
      .GreaterThan(0).WithMessage("Price must be greater than 0");

    RuleFor(x => x.CodeInternal)
      .NotEmpty().WithMessage("Internal code is required")
      .MaximumLength(50).WithMessage("Internal code must not exceed 50 characters");

    RuleFor(x => x.Year)
      .GreaterThan(1800).WithMessage("Year must be greater than 1800")
      .LessThanOrEqualTo(System.DateTime.Now.Year).WithMessage("Year cannot be in the future");

    RuleFor(x => x.IdOwner)
      .NotEmpty().WithMessage("Owner ID is required")
      .MustAsync(async (id, cancellation) =>
      {
        var owner = await _ownerRepository.GetByIdAsync(id);
        return owner != null;
      }).WithMessage("Owner not found");
  }
}