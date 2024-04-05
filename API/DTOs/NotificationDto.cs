namespace AsparagusN.DTOs;

public class NotificationDto
{
    public string UserEmail { get; set; }
    public string ArabicContent { get; set; }
    public string EnglishContent { get; set; }
    public DateTime CreatedAt { get; set; }
}