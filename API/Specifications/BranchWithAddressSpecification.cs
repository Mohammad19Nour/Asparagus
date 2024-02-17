using System.Linq.Expressions;
using AsparagusN.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class BranchWithAddressSpecification : BaseSpecification<Branch>
{
    public BranchWithAddressSpecification() : base(x => true)
    {
        AddInclude(x=>x.Include(y=>y.Address));
    }
    public BranchWithAddressSpecification(int branchId) : base(x => x.Id == branchId)
    {
        AddInclude(x=>x.Include(y=>y.Address));
    }
}