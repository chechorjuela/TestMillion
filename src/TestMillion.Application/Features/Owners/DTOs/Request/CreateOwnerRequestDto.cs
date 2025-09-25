namespace TestMillion.Application.Features.Owners.DTOs.Request;

public class CreateOwnerRequestDto
{
  public string Name { get; set; } = string.Empty;
  public string Address { get; set; } = string.Empty;
  public string Photo { get; set; } = string.Empty;
  public DateOnly Birthdate { get; set; }
}