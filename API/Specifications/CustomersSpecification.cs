using System.Linq.Expressions;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace AsparagusN.Specifications;

public class CustomersSpecification : BaseSpecification<AppUser>
{
    public CustomersSpecification(bool includeAddress = true)
        : base(x => x.UserRoles.Count == 1 && x.UserRoles.First().Role.Name!.ToLower() == Roles.User.GetDisplayName().ToLower())
    {
        if (includeAddress)
        {
            AddInclude(x => x.Include(y => y.HomeAddress));
            AddInclude(x => x.Include(y => y.WorkAddress));
        }

        AddInclude(x => x.Include(y => y.UserRoles).ThenInclude(r => r.Role));
    }
    
    
}