using System.ComponentModel.DataAnnotations;

namespace TestMillion.Application.Common.Models;

public class PaginationRequestDto
{
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 10;
}