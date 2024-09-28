using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class AddressDto
{
    [Required]      // annotation gives us the ability to return ModelState Errors in their correct format
    public string Line1 { get; set; } = string.Empty;

    public string? Line2 { get; set; }

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string State { get; set; } = string.Empty;

    [Required]
    public string PostalCode { get; set; } = string.Empty;

    [Required]
    public string Country { get; set; } = string.Empty;
}
