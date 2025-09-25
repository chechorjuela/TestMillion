using TestMillion.Application.Common.Models;

namespace TestMillion.Application.Common.Queries;

public abstract class PaginatedQuery
{
    public PaginationRequestDto Pagination { get; set; } = new();
    public FilterRequestDto Filter { get; set; } = new();
}
