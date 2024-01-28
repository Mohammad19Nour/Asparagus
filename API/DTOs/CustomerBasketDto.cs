using System.ComponentModel.DataAnnotations;

namespace AsparagusN.DTOs;

public class CustomerBasketDto
{
    [Required]
    public string Id { get; set; }
    public List<BasketItemDto> Items { get; set; }
}