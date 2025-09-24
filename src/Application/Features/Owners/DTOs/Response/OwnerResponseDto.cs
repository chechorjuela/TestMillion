namespace TestMillion.Application.Features.Owners.DTOs.Response;

public class OwnerResponseDto
{
  public string Id { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public string Address { get; set; } = string.Empty;
  public string Photo { get; set; } = string.Empty;
  public DateOnly Birthdate { get; set; }
}
