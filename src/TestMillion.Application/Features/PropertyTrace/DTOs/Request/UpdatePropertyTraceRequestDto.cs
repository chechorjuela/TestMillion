namespace TestMillion.Application.Features.PropertyTrace.DTOs.Request;

public class UpdatePropertyTraceRequestDto
{
    public DateOnly DateSale { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public decimal Tax { get; set; }
    public string IdProperty { get; set; } = string.Empty;
}
