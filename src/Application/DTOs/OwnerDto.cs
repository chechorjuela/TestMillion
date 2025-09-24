namespace TestMillion.Application.DTOs;

public class OwnerDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateOnly Birthdate { get; set; }
}