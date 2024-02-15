using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AsparagusN.Enums;

namespace AsparagusN.DTOs.CouponDtos;

public class UpdateCouponDto
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    [Range(0.01,double.MaxValue,ErrorMessage = "Value must be greater than 0")]
    public decimal? Value { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AppCouponType? Type { get; set; }
}