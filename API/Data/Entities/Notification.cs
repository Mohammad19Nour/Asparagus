using AsparagusN.Data.Entities.Identity;

namespace AsparagusN.Data.Entities;

public class Notification
{
    public int Id { get; set; }
    public string UserEmail { get; set; }
    public string ArabicContent { get; set; } = "";
    public string EnglishContent { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsSent { get; set; } = false;
}