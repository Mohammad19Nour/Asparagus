namespace AsparagusN.DTOs.BasketDtos;

public class CustomerBasketDto
{
    public int Id { get; set; }
    public List<BasketItemDto> Items { get; set; }

    public decimal TotalPrice()
    {
        return Items.Sum(x => x.Quantity * 
                              (x.PricePerCarb * x.AddedCarb + x.AddedProtein * x.PricePerProtein + x.Price) );
    }
}