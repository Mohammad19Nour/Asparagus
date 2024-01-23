using Microsoft.AspNetCore.Identity;

namespace AsparagusN.Entities.Identity;

public class AppUserRole : IdentityUserRole<int>
{
    public AppUser User { get; set; }
    public AppRole Role { get; set; }
}