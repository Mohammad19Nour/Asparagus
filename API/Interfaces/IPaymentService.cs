using AsparagusN.Data.Entities;

namespace AsparagusN.Interfaces;

public interface IPaymentService
{
    Task<(bool Success,string Message)> CheckPaymentStatus(string transactionNo,double orderPrice);
}