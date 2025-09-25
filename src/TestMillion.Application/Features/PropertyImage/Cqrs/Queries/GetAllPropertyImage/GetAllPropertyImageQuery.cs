using TestMillion.Application.Common.Queries;
using TestMillion.Application.Common.Response;
using TestMillion.Application.Features.PropertyImage.DTOs.Response;

namespace TestMillion.Application.Features.PropertyImage.Cqrs.Queries.GetAllPropertyImage;

public class GetAllPropertyImageQuery : PaginatedQuery, IQuery<PagedResponse<List<PropertyImageResponseDto>>>
{
}
