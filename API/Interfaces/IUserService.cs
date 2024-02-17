using AsparagusN.Data.Entities.Identity;

namespace AsparagusN.Interfaces;

public interface IUserService
{
    Task<AppUser?> GetUserFromContextAsync(HttpContext httpContext);
}