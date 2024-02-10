namespace AsparagusN.Data.Entities;

public class CustomerBasket
{
    public CustomerBasket(int  id)
    {
        Id = id;
    }

    public CustomerBasket()
    {
    }

    public int Id { get; set; }
    public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    public decimal TotalPrice { get; set; }
}