﻿using System.Linq.Expressions;
using AsparagusN.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class DriverSpecification : BaseSpecification<Driver>
{
    public DriverSpecification(bool activeOnly = false) : base(x=>(!activeOnly || x.IsActive))
    {
        AddInclude(x=>x.Include(y=>y.Zone));
    }
    
    public DriverSpecification(int driverId) : base(x=>x.Id == driverId)
    {
        AddInclude(x=>x.Include(y=>y.Zone));
    }
    public DriverSpecification(string email) : base(x=>x.Email.ToLower() == email)
    {
        AddInclude(x=>x.Include(y=>y.Zone));
    }
}