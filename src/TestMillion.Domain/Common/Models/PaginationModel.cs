using System.ComponentModel.DataAnnotations;

namespace TestMillion.Domain.Common.Models;

public class PaginationModel
{
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 10;
}

public class FilterModel
{
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; }
    public Dictionary<string, string> Filters { get; set; } = new();
}

public class PaginatedFilterModel
{
    public PaginationModel Pagination { get; set; } = new();
    public FilterModel Filter { get; set; } = new();
}