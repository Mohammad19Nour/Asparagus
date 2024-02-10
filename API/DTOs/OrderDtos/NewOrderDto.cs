namespace AsparagusN.DTOs.OrderDtos;

public class NewOrderDto
{
    public int BasketId { get; set; }
    public AddressDto ShipToAddress { get; set; }
}