namespace AsparagusN.Interfaces;

public interface INotificationService
{
    public Task<bool> NotifyUserByEmail(string userEmail, string arabicContent, string englishContent);
    public Task<bool> NotifyAllMealPlanUsers(string arabicContent, string englishContent);
    public Task<bool> NotifyAllNormalUsers(string arabicContent, string englishContent);
}