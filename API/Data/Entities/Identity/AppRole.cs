using Microsoft.AspNetCore.Identity;

namespace AsparagusN.Data.Entities.Identity;

public class AppRole : IdentityRole<int>    
{
    public ICollection<AppUserRole> UserRoles { get; set; }
}