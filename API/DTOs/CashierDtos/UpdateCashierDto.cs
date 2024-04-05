using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AsparagusN.Enums;

namespace AsparagusN.DTOs.CashierDtos;

public class UpdateCashierDto
{
    [EmailAddress]  public string? Email { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }
    public int? BranchId { get; set; }
    public bool? IsActive { get; set; }
    public IFormFile? Image { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Period? Period { get; set; }
}