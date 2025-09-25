using TestMillion.Application.Features.Properties.Cqrs.Queries.GetProperties;

namespace TestMillion.Application.Properties.Cqrs.Queries.GetProperties;

public class GetPropertyAllQueryValidator : AbstractValidator<GetPropertyAllQuery>
{
    public GetPropertyAllQueryValidator()
    {
        RuleFor(x => x.Filter.MinPrice)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Filter.MinPrice.HasValue)
            .WithMessage("Minimum price cannot be negative");

        RuleFor(x => x.Filter.MaxPrice)
            .GreaterThanOrEqualTo(x => x.Filter.MinPrice ?? 0)
            .When(x => x.Filter.MaxPrice.HasValue)
            .WithMessage("Maximum price must be greater than or equal to minimum price");
    }
}