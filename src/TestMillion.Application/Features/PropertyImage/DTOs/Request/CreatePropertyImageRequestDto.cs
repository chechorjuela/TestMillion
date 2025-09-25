using System.Text.Json.Serialization;

namespace TestMillion.Application.Features.PropertyImage.DTOs.Request;

public class CreatePropertyImageRequestDto
{
    public string File { get; set; } = string.Empty;
    public bool Enabled { get; set; }
    [JsonPropertyName("idProperty")]
    public string IdProperty { get; set; } = string.Empty;
}
