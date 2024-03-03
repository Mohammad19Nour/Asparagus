using System.Linq.Expressions;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace AsparagusN.Specifications;

public class EmployeesSpecification : BaseSpecification<AppUser>
{
    public EmployeesSpecification() 
        : base(u=>u.UserRoles.Select(t=>t.Role.Name.ToLower()).Contains( Roles.Employee.GetDisplayName().ToLower()))
    {
        AddInclude(u=>u.Include(r=>r.UserRoles).ThenInclude(v=>v.Role));
    }

    public EmployeesSpecification(string email)
        : base(u =>
            u.Email.ToLower() == email&&
            u.UserRoles.Select(t=>t.Role.Name.ToLower()).Contains( Roles.Employee.GetDisplayName().ToLower()))
    {
        AddInclude(u=>u.Include(r=>r.UserRoles).ThenInclude(v=>v.Role));
    }
    public EmployeesSpecification(int id)
        : base(u =>
            u.Id == id &&
            u.UserRoles.Select(t=>t.Role.Name.ToLower()).Contains( Roles.Employee.GetDisplayName().ToLower()))
    {
        AddInclude(u=>u.Include(r=>r.UserRoles).ThenInclude(v=>v.Role));
    }
}