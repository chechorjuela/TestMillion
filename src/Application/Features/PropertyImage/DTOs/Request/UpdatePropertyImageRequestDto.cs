namespace TestMillion.Application.Features.PropertyImage.DTOs.Request;

public class UpdatePropertyImageRequestDto
{
    public string? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool Enabled { get; set; }
    public string ImagePath { get; set; } = string.Empty;
}
