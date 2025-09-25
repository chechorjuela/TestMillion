using FluentValidation;
using TestMillion.Application.Common;
using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces.Base;

namespace TestMillion.Application.Properties.DTOs.Request.Validators;

public class CreatePropertyRequestDtoValidator : AbstractValidator<CreatePropertyRequestDto>
{
  private readonly IBaseRepository<Owner> _ownerRepository;

  public CreatePropertyRequestDtoValidator(IBaseRepository<Owner> ownerRepository)
  {
    _ownerRepository = ownerRepository;

    RuleFor(x => x.Name)
      .NotEmpty().WithMessage("El nombre es requerido")
      .MaximumLength(100).WithMessage("El nombre no debe exceder los 100 caracteres");

    RuleFor(x => x.Address)
      .NotEmpty().WithMessage("La dirección es requerida")
      .MaximumLength(200).WithMessage("La dirección no debe exceder los 200 caracteres");

    RuleFor(x => x.Price)
      .GreaterThan(0).WithMessage("El precio debe ser mayor que 0")
      .LessThan(1000000000).WithMessage("El precio no puede ser mayor a 1.000.000.000");

    RuleFor(x => x.CodeInternal)
      .NotEmpty().WithMessage("El código interno es requerido")
      .MaximumLength(50).WithMessage("El código interno no debe exceder los 50 caracteres")
      .Matches(@"^[a-zA-Z0-9-]+$").WithMessage("El código interno solo puede contener letras, números y guiones");

    RuleFor(x => x.Year)
      .GreaterThan(1800).WithMessage("El año debe ser mayor a 1800")
      .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("El año no puede ser futuro");

    RuleFor(x => x.IdOwner)
      .NotEmpty().WithMessage("El ID del propietario es requerido")
      .MustAsync(ValidateOwnerExists).WithMessage("El propietario especificado no existe");
  }

  private async Task<bool> ValidateOwnerExists(string ownerId, CancellationToken cancellation)
  {
    if (string.IsNullOrEmpty(ownerId)) return false;
    var owner = await _ownerRepository.GetByIdAsync(ownerId);
    return owner != null;
  }
}