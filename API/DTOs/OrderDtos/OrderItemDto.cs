namespace AsparagusN.DTOs.OrderDtos;

public class OrderItemDto
{
    public int Id { get; set; }
    public MealItemOrderedDto OrderedMeal { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}