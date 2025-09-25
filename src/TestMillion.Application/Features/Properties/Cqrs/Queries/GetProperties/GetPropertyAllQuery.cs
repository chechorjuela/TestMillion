using TestMillion.Application.Common.Queries;
using TestMillion.Application.Common.Response;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Application.Features.Properties.DTOs.Request;
using TestMillion.Domain.Common.Models;

namespace TestMillion.Application.Features.Properties.Cqrs.Queries.GetProperties;

public class GetPropertyAllQuery : PaginatedQuery, IQuery<PagedResponse<List<PropertyResponseDto>>>
{
    public new PropertyFilterDto Filter { get; set; } = new();
}
