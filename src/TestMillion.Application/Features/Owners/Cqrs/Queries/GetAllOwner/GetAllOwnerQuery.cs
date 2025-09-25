using TestMillion.Application.Common.Queries;
using TestMillion.Application.Common.Response;
using TestMillion.Application.Features.Owners.DTOs.Response;

namespace TestMillion.Application.Features.Owners.Cqrs.Queries.GetAllOwner;

public class GetAllOwnerQuery : PaginatedQuery, IQuery<PagedResponse<List<OwnerResponseDto>>>
{
}
