using System.Linq.Expressions;
using AsparagusN.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class ZoneWithDriversSpecification : BaseSpecification<Zone>
{
    public ZoneWithDriversSpecification(int id) : base(x=>x.Id == id)
    {
        AddInclude(x=>x.Include(y=>y.Drivers));
    }
    
    public ZoneWithDriversSpecification() : base(x=>true)
    {
        AddInclude(x=>x.Include(y=>y.Drivers));
    }
}