﻿using System.Linq.Expressions;
using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class OrderWithItemsSpecification : BaseSpecification<Order>
{
    public OrderWithItemsSpecification(string email) : base(x => x.BuyerEmail == email)
    {
        
        AddInclude(x => x.Include(y => y.Driver));
        AddInclude(x => x.Include(y => y.Branch));
        AddInclude(x => x.Include(y => y.Items).ThenInclude(y=>y.OrderedMeal));
        AddInclude(x => x.Include(y => y.ShipToAddress));
    }

    public OrderWithItemsSpecification(string email, int id) : base(x => x.Id == id
                                                                         && email == x.BuyerEmail)
    {
        
        AddInclude(x => x.Include(y => y.Driver));
        AddInclude(x => x.Include(y => y.Branch));
        AddInclude(x => x.Include(y => y.Items).ThenInclude(y=>y.OrderedMeal));
        AddInclude(x => x.Include(y => y.ShipToAddress));
    }
    public OrderWithItemsSpecification(OrderStatus status) : base(x => x.Status == status)
    {
        
        AddInclude(x => x.Include(y => y.Driver));
        AddInclude(x => x.Include(y => y.Branch));
        AddInclude(x => x.Include(y => y.Items).ThenInclude(y=>y.OrderedMeal));
        AddInclude(x => x.Include(y => y.ShipToAddress));
    }
    public OrderWithItemsSpecification( int id) : base(x => x.Id == id)
    {
        AddInclude(x => x.Include(y => y.Driver));
        AddInclude(x => x.Include(y => y.Branch));
        AddInclude(x => x.Include(y => y.Items).ThenInclude(y=>y.OrderedMeal));
        AddInclude(x => x.Include(y => y.ShipToAddress));
    }
}