using TestMillion.Application.Features.Owners.DTOs.Response;
using TestMillion.Application.Features.PropertyImage.DTOs.Response;

namespace TestMillion.Application.Features.Properties.DTOs.Response;

public class PropertyDetailDto : PropertyResponseDto
{
    public new string? MainImage { get; set; }
    public IEnumerable<PropertyImageResponseDto> Images { get; set; } = Array.Empty<PropertyImageResponseDto>();
}