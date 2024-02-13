﻿namespace AsparagusN.Data.Entities;

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
   
    
    public decimal TotalPrice()
    {
        return Items.Sum(x => x.Quantity * 
                              (x.PricePerCarb * x.AddedCarb + x.AddedProtein * x.PricePerProtein + x.Price) );
    }
}