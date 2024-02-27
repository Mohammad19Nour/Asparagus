using AsparagusN.Data.Entities;

namespace AsparagusN.Interfaces;

public interface ICarService
{
    public Task<Car?> GetCarByIdAsync(int carId, bool include = false);
    public Task<List<Car>> GetAllCarsAsync();
    public Task<bool> IsCarAvailable(int carId, DateTime startTime, DateTime endTime);
    public Task<(bool Success,string Message)> MakeBooking( int userId, DateTime startTime, DateTime endTime);
    public Task<(int? CarId,string Message)> GetAvailableCarId(DateTime startTime, DateTime endTime);
    public Task<(int? carId,DateTime? StartAvailableTime, string Message)> GetAlternativeBookingTime(DateTime startTime, DateTime endTime);
}