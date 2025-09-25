using TestMillion.Domain.Common.Entities;

namespace TestMillion.Application.Features.Properties.DTOs.Response;

public class OwnerResponseDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateOnly Birthday { get; set; }
    public string Photo { get; set; } = string.Empty;
}