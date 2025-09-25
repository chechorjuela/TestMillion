using TestMillion.Application.Features.Owners.DTOs.Response;

namespace TestMillion.Application.Features.Properties.DTOs.Response;

public class PropertyResponseDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CodeInternal { get; set; } = string.Empty;
    public int Year { get; set; }
    public string IdOwner { get; set; } = string.Empty;
    public OwnerResponseDto? Owner { get; set; }
    public string? MainImage { get; set; }
}
