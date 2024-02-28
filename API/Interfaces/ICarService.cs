using AsparagusN.Data.Entities;
using AsparagusN.DTOs.CarDtos;

namespace AsparagusN.Interfaces;

public interface ICarService
{
    public Task<(Car? car, string Message)> AddCar(NewCarDto carDto); 
    
    public Task<(Car? car, string Message)> UpdateCar(UpdateCarDto dto, int carId); 
    public Task<Car?> GetCarByIdAsync(int carId, bool include = false);
    public Task<List<Car>> GetAllCarsAsync();
    public Task<bool> IsCarAvailable(int carId, DateTime startTime, DateTime endTime);
    public Task<(bool Success,string Message)> MakeBooking( int userId, DateTime startTime);
    public Task<(int? CarId,string Message)> GetAvailableCarId(DateTime startTime, DateTime endTime);
    public Task<(int? carId,DateTime? StartAvailableTime, string Message)> GetAlternativeBookingTime(DateTime startTime, DateTime endTime);
}