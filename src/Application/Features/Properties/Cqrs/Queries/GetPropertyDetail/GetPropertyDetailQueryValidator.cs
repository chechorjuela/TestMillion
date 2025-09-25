using TestMillion.Application.Features.Properties.Cqrs.Queries.GetPropertyDetail;

namespace TestMillion.Application.Properties.Cqrs.Queries.GetPropertyDetail;

public class GetPropertyDetailQueryValidator : AbstractValidator<GetPropertyDetailQuery>
{
  public GetPropertyDetailQueryValidator()
  {
    RuleFor(x => x.Id)
      .NotEmpty()
      .WithMessage("Property ID is required");
  }
}