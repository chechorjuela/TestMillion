namespace TestMillion.Application.Common.Models;

public class PaginatedFilterRequestDto
{
    public PaginationRequestDto Pagination { get; set; } = new();
    public FilterRequestDto Filter { get; set; } = new();
}