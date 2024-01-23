using Microsoft.AspNetCore.Identity;

namespace AsparagusN.Entities.Identity;

public class AppRole : IdentityRole<int>    
{
    public ICollection<AppUserRole> UserRoles { get; set; }
}