using System.Linq.Expressions;
using AsparagusN.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class CarSpecification : BaseSpecification<Car>
{
    public CarSpecification(int id) : base(x=>x.Id == id)
    {
        AddInclude(c=>c.Include(y=>y.WorkingDays));
        AddInclude(c=>c.Include(y=>y.Bookings));
    }
    public CarSpecification() : base(x=>true)
    {
        AddInclude(c=>c.Include(y=>y.WorkingDays));
        AddInclude(c=>c.Include(y=>y.Bookings));
    }
}