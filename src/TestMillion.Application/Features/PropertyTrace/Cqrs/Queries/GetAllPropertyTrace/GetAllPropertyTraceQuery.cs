using TestMillion.Application.Common.Queries;
using TestMillion.Application.Common.Response;
using TestMillion.Application.Features.PropertyTrace.DTOs.Response;

namespace TestMillion.Application.Features.PropertyTrace.Cqrs.Queries.GetAllPropertyTrace;

public class GetAllPropertyTraceQuery : PaginatedQuery, IQuery<PagedResponse<List<PropertyTraceResponseDto>>>
{
}
