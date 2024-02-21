using System.Linq.Expressions;
using AsparagusN.Data.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class CustomersSpecification : BaseSpecification<AppUser>
{
    public CustomersSpecification()
        : base(x => x.UserRoles.Count == 1 && x.UserRoles.First().Role.Name!.ToLower() == "user")
    {
        AddInclude(x=>x.Include(y=>y.HomeAddress));
        AddInclude(x=>x.Include(y=>y.WorkAddress));
        AddInclude(x => x.Include(y => y.UserRoles).ThenInclude(r => r.Role));
    }
}