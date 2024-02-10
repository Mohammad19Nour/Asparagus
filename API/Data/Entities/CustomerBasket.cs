namespace AsparagusN.Entities;

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
}