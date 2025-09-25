using TestMillion.Application.Common.Queries;
using TestMillion.Application.Features.Properties.DTOs.Response;

namespace TestMillion.Application.Features.Properties.Cqrs.Queries.GetPropertyDetail;

public class GetPropertyDetailQuery : IQuery<PropertyDetailDto>
{
    public string Id { get; set; } = string.Empty;
}