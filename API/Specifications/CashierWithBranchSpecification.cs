using System.Linq.Expressions;
using AsparagusN.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class CashierWithBranchSpecification : BaseSpecification<Cashier>
{
    public CashierWithBranchSpecification(bool activeOnly = false) : base(x=>(!activeOnly || x.IsActive))
    {
        AddInclude(x=>x.Include(y=>y.Branch));
    }
    
    public CashierWithBranchSpecification(int driverId) : base(x=>x.Id == driverId)
    {
        AddInclude(x=>x.Include(y=>y.Branch));
    }
}