using System.Linq.Expressions;
using AsparagusN.Data.Entities;

namespace AsparagusN.Specifications;

public class UnsentNotificationsForUserSpecification : BaseSpecification<Notification>
{
    public UnsentNotificationsForUserSpecification(string userEmail) 
        : base(x=>!x.IsSent && x.UserEmail.ToLower() == userEmail.ToLower())
    {
    }
}