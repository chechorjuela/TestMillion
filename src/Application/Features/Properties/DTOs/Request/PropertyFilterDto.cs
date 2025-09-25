using TestMillion.Application.Common.Models;

namespace TestMillion.Application.Features.Properties.DTOs.Request;

public class PropertyFilterDto : FilterRequestDto
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}