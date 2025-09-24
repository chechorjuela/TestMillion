namespace TestMillion.Application.Features.Owners.DTOs.Request;

public class UpdateOwnerRequestDto
{
    public required string? Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string Photo { get; set; }
    public DateOnly Birthdate { get; set; }
}