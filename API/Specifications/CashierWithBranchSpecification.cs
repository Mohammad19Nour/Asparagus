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
    
    public CashierWithBranchSpecification(int cashierId) : base(x=>x.Id == cashierId)
    {
        AddInclude(x=>x.Include(y=>y.Branch));
    }
    public CashierWithBranchSpecification(string email) : base(x=>x.Email == email)
    {
        AddInclude(x=>x.Include(y=>y.Branch));
    }
}