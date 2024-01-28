using System.Linq.Expressions;
using AsparagusN.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications.UserSpecifications;

public class UserWithAddressSpecification : BaseSpecification<AppUser>
{
    public UserWithAddressSpecification(string email) : base(x => x.Email == email)
    {
        AddInclude(x=>x.Include(y=>y.WorkAddress));
        AddInclude(x=>x.Include(y=>y.HomeAddress));
    }
}