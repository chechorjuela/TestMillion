using System.ComponentModel.DataAnnotations;

namespace TestMillion.Application.Common.Models;

public class FilterRequestDto
{
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; }
    public Dictionary<string, string> Filters { get; set; } = new();
}