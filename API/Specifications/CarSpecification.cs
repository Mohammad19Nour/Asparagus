using System.Linq.Expressions;
using AsparagusN.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class CarSpecification : BaseSpecification<Car>
{
    public CarSpecification(int id) : base(x => x.Id == id)
    {
        AddInclude(c => c.Include(y => y.WorkingDays));
        AddInclude(c => c.Include(y => y.Bookings));
    }

    public CarSpecification(string city, bool includeBooking = true) : base(x => x.City.ToLower() == city.ToLower())
    {
        AddInclude(c => c.Include(y => y.WorkingDays));
        if (includeBooking)
            AddInclude(c => c.Include(y => y.Bookings).ThenInclude(u => u.User));
    }
    public CarSpecification( bool includeBooking = true) : base(x => true)
    {
        AddInclude(c => c.Include(y => y.WorkingDays));
        if (includeBooking)
            AddInclude(c => c.Include(y => y.Bookings).ThenInclude(u => u.User));
    }
}