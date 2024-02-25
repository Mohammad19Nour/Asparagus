namespace AsparagusN.Data.Entities.OrderAggregate;

public class OrderItem
{
    public int Id { get; set; }
    public MealItemOrdered OrderedMeal { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int PointsPrice { get; set; }
    public int GainedPoint { get; set; }
}