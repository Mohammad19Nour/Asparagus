namespace AsparagusN.DTOs;

public class NewNotificationDto
{
    public string ArabicContent { get; set; }
    public string EnglishContent { get; set; }
    public string? UserEmail { get; set; } // for single user notification  type
}