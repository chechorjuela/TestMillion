using Microsoft.Extensions.Logging;
using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces;
using TestMillion.Domain.Interfaces.Base;

namespace TestMillion.Application.Features.Properties.Cqrs.Commands.CreateProperty;

public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
{
  private readonly IOwnerRepository _ownerRepository;
  private readonly ILogger<CreatePropertyCommandValidator> _logger;

  public CreatePropertyCommandValidator(IOwnerRepository ownerRepository, ILogger<CreatePropertyCommandValidator> logger)
  {
    _ownerRepository = ownerRepository;
    _logger = logger;

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
      .Cascade(CascadeMode.Stop)
      .NotEmpty().WithMessage("Owner ID is required")
      .Must(id => 
      {
        var isValid = MongoDB.Bson.ObjectId.TryParse(id, out _);
        _logger.LogInformation("Validating owner ID format: {OwnerId}, IsValid: {IsValid}", id, isValid);
        return isValid;
      })
      .WithMessage("Owner ID must be a valid 24-character hexadecimal string")
      .MustAsync(async (id, cancellation) =>
      {
        _logger.LogInformation("Looking up owner with ID: {OwnerId}", id);
        var owner = await _ownerRepository.GetByIdAsync(id);
        _logger.LogInformation("Owner lookup result for ID {OwnerId}: {Found}", id, owner != null);
        return owner != null;
      })
      .WithMessage("Owner not found");
  }
}