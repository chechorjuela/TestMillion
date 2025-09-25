using TestMillion.Application.Features.Properties.DTOs.Response;

namespace TestMillion.Application.Features.PropertyImage.DTOs.Response;

public class PropertyImageResponseDto
{
    public string? Id { get; set; }
    public PropertyResponseDto Property { get; set; } = null!;
    public bool Enabled { get; set; }
    public string? FileUrl { get; set; }
}
