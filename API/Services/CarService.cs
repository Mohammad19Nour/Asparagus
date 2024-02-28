using AsparagusN.Data;
using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.DTOs;
using AsparagusN.DTOs.CarDtos;
using AsparagusN.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Services;

public class CarService : ICarService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CarService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<(Car? car, string Message)> AddCar(NewCarDto carDto)
    {
        var car = _mapper.Map<Car>(carDto);
        _unitOfWork.Repository<Car>().Add(car);

        if (await _unitOfWork.SaveChanges()) return (car, "Done");
        return (null, "Something happened");
    }

    public async Task<(Car? car, string Message)> UpdateCar(UpdateCarDto dto, int carId)
    {
        if (dto.WorkingStartHour > dto.WorkingEndHour)
            return (null, "start working hour should be less than end working hour");
        var car = await GetCarByIdAsync(carId);
        if (car == null) return (null, "Car not found");
        Console.WriteLine(dto.WorkingStartHour);
        _mapper.Map(dto, car);

        _unitOfWork.Repository<Car>().Update(car);
        if (await _unitOfWork.SaveChanges())
            return (car, "Done");
        return (null, "Failed to update. Something happened during updating car");
    }

    public async Task<Car?> GetCarByIdAsync(int carId, bool include = false)
    {
        try
        {
            Car? car = car = await _unitOfWork.Repository<Car>().GetQueryable().Include(x => x.Bookings)
                .Include(y => y.WorkingDays).Where(c => c.Id == carId).FirstOrDefaultAsync();


            return car;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<Car>> GetAllCarsAsync()
    {
        try
        {
            return (await _unitOfWork.Repository<Car>().GetQueryable().Include(x=>x.WorkingDays).ToListAsync()).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> IsCarAvailable(int carId, DateTime startTime, DateTime endTime)
    {
        var car = await GetCarByIdAsync(carId);
        if (car == null) return false;

        if (car.WorkingStartHour > startTime.TimeOfDay || car.WorkingEndHour < endTime.TimeOfDay)
            return false;

        var booking = car.Bookings.FirstOrDefault(b => (b.StartTime <= startTime && b.EndTime >= endTime) ||
                                                       (b.StartTime <= endTime && startTime <= b.StartTime) ||
                                                       (b.EndTime <= endTime && startTime <= b.EndTime));
        return booking == null;
    }

    public async Task<(bool Success, string Message)> MakeBooking(int userId, DateTime startTime)
    {
        DateTime endTime = startTime.AddHours(2);
        var user = await _unitOfWork.Repository<AppUser>().GetByIdAsync(userId);

        if (user == null) return (false, "User not found");

        var (carId, message) = await GetAvailableCarId(startTime, endTime);
        if (carId != null)
        {
            var car = await GetCarByIdAsync(carId.Value);

            var booking = new Booking
            {
                User = user,
                UserId = userId,
                StartTime = startTime,
                CarId = carId.Value
            };
            car.Bookings.Add(booking);

            if (await _unitOfWork.SaveChanges())
                return (true, "Done");
            return (false, "Failed to make booking... something wrong happened");
        }
        else
        {
            (carId, var availableTime, message) = await GetAlternativeBookingTime(startTime, endTime);

            if (carId == null)
                return (false, message);

            return (false, $"Your booked time is not available... you could book time {availableTime} ");
        }
    }

    public async Task<(int? CarId, string Message)> GetAvailableCarId(DateTime startTime, DateTime endTime)
    {
        try
        {
            var cars = await GetAllCarsAsync();
            foreach (var car in cars)
            {
                if (await IsCarAvailable(car.Id, startTime, endTime))
                    return (car.Id, "Ok");
            }

            // no car available at this range time
            return (null, "No car available at this range time");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<(int? carId, DateTime? StartAvailableTime, string Message)> GetAlternativeBookingTime(
        DateTime startTime, DateTime endTime)
    {
        throw new NotImplementedException();
    }
}