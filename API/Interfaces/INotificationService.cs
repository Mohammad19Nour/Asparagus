using AsparagusN.Data.Entities;

namespace AsparagusN.Interfaces;

public interface INotificationService
{
    public Task<bool> NotifyUserByEmail(string userEmail, string arabicContent, string englishContent);
    public Task<bool> NotifyAllMealPlanUsers(string arabicContent, string englishContent);
    public Task<bool> NotifyAllNormalUsers(string arabicContent, string englishContent);
    public Task<List<Notification>> GetUnsentNotificationsForUserAsync(string userEmail);
    public Task<List<Notification>> GetAllNotificationsForUserAsync(string userEmail);
    public Task<bool> MarkSent(List<Notification> notifications);
}