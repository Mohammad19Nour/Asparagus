namespace AsparagusN.DTOs.BasketDtos;

public class CustomerBasketDto
{
    public int Id { get; set; }
    public List<BasketItemDto> Items { get; set; }

    public decimal TotalPrice { get; set; }
}