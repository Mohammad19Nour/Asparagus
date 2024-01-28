using System.Security.Claims;

namespace AsparagusN.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetEmail(this ClaimsPrincipal user)
    {
        return user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value.ToLower();
    }
}