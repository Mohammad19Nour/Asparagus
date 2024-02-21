﻿using System.Linq.Expressions;
using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class OrdersForDriverWithStatusSpecification : BaseSpecification<Order>
{
    public OrdersForDriverWithStatusSpecification(int driverId,OrderStatus status) 
        : base(x=>x.DriverId == driverId && status == x.Status)
    {
        AddInclude(x=>x.Include(y=>y.ShipToAddress));
    }
}