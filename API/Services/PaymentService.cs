using AsparagusN.Data.Entities;
using AsparagusN.Interfaces;
using Newtonsoft.Json;
using Stripe;
using Order = AsparagusN.Data.Entities.OrderAggregate.Order;

namespace AsparagusN.Services;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public PaymentService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }


    public async Task<(bool Success, string Message)> CheckPaymentStatus(string transactionNo, double orderPrice)
    {
        var orders = await _unitOfWork.Repository<Order>().ListAllAsync();
        if (orders.FirstOrDefault(c => c.BillId == transactionNo) != null)
            return (false, "Not a valid transaction_no"); 
        var httpClient = new HttpClient();
        
        httpClient.BaseAddress = new Uri(_configuration["FoloosiUrl"]);
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Add("secret_key", _configuration["FoloosiSecretKey"]);
        var response = await httpClient.GetAsync(transactionNo);
        Console.WriteLine(response);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(responseBody);
            string status = json["data"]["status"];
            if (status.ToLower() != "success")
            {
                string reason = json["data"]["payment_failed_reason"];
                return (false, reason);
            }

            string email = json["data"]["customer"]["email"];
            double amount = json["data"]["merchant_amount"];

            if (amount < orderPrice)
                return (false, "Paid price is lees than the order price");
            return (true, "Done");
        }

        return (false, "Not authorized");
    }
}