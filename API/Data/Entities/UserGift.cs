using AsparagusN.Data.Entities.Identity;

namespace AsparagusN.Data.Entities;

public class UserGift
{
    public int Id { get; set; }
    public AppUser User { get; set; }
    public int AppUserId { get; set; }
    public bool IsSent { get; set; } = false;
}