using System.ComponentModel.DataAnnotations;

namespace TestMillion.Application.Properties.DTOs.Request;

public class CreatePropertyRequest
{
    [Required(ErrorMessage = "IdOwner is required")]
    public string IdOwner { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Price is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "CodeInternal is required")]
    public string CodeInternal { get; set; } = string.Empty;
    
    [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100")]
    public int Year { get; set; }
    
    public string? MainImage { get; set; }
}