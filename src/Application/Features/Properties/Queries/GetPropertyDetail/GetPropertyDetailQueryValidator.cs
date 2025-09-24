using FluentValidation;

namespace TestMillion.Application.Properties.Queries.GetPropertyDetail;

public class GetPropertyDetailQueryValidator : AbstractValidator<GetPropertyDetailQuery>
{
    public GetPropertyDetailQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Property ID is required");
    }
}