using System.Globalization;
using AsparagusN.Data;
using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.DTOs;
using AsparagusN.DTOs.CarDtos;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
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
        if (dto.WorkingStartHour != null && dto.WorkingEndHour != null &&
            TimeSpan.Parse(dto.WorkingStartHour) > TimeSpan.Parse(dto.WorkingEndHour))
            return (null, "start working hour should be less than end working hour");
        var car = await GetCarByIdAsync(carId);
        if (car == null) return (null, "Car not found");
        _mapper.Map(dto, car);

        if (dto.WorkingStartHour != null) car.WorkingStartHour = TimeSpan.Parse(dto.WorkingStartHour);
        if (dto.WorkingEndHour != null) car.WorkingEndHour = TimeSpan.Parse(dto.WorkingEndHour);
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
            return (await _unitOfWork.Repository<Car>().GetQueryable().Include(x => x.WorkingDays).ToListAsync())
                .ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public bool IsCarAvailable(Car car, DateTime startTime, DateTime endTime)
    {
        if (startTime.DayOfWeek != endTime.DayOfWeek) return false;
        if (car.WorkingStartHour> startTime.TimeOfDay ||
            car.WorkingEndHour < endTime.TimeOfDay)
            return false;

        if (car.WorkingDays.All(x => x.Day != startTime.DayOfWeek))
            return false;

        var bookingsStart = car.Bookings.Select(c => c.StartTime).ToList();

        var booking = car.Bookings.FirstOrDefault(b => (b.StartTime <= startTime && b.EndTime >= endTime) ||
                                                       (b.StartTime < endTime && startTime < b.StartTime) ||
                                                       (b.EndTime < endTime && startTime < b.EndTime));
        return booking == null;
    }

    public async Task<(bool Success, string Message)> MakeBooking(int userId, DateTime startTime)
    {
        DateTime endTime = startTime.AddHours(2);
        var user = await _unitOfWork.Repository<AppUser>().GetByIdAsync(userId);

        if (user == null) return (false, "User not found");

        var available = await GetAvailableDates();
        var rangeList = available.FirstOrDefault(x => x.Any(t => t.Start == startTime && t.End == endTime));
        if (rangeList == null)
            return (false, "This time is not available");

        var (carId, message) = await GetAvailableCarId(startTime, endTime);
        if (carId != null)
        {
            var car = await GetCarByIdAsync(carId.Value);

            var booking = new Booking
            {
                User = user,
                UserId = userId,
                StartTime = startTime,
                CarId = carId.Value,
                EndTime = endTime
            };
            car.Bookings.Add(booking);

            if (await _unitOfWork.SaveChanges())
                return (true, "Done");
            return (false, "Failed to make booking... something wrong happened");
        }

        return (false, $"Your booked time is not available...");
    }

    public async Task<(int? CarId, string Message)> GetAvailableCarId(DateTime startTime, DateTime endTime)
    {
        try
        {
            var cars = await GetAllCarsAsync();
            foreach (var car in cars)
            {
                if (IsCarAvailable(car, startTime, endTime))
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

    public async Task<List<List<(DateTime Start, DateTime End)>>> GetAvailableDates()
    {
        var days = new List<List<(DateTime Start, DateTime End)>>();
        var carSpec = new CarSpecification();
        var cars = await _unitOfWork.Repository<Car>().ListWithSpecAsync(carSpec);

        for (int j = 0; j < 3; j++)
        {
            var day = DateTime.Now.Date.AddDays(j);
            var availableInDay = new List<(DateTime Start, DateTime End)>();

            foreach (var car in cars)
            {
                for (int w = 0; w <= 22; w++)
                {
                    if (!IsCarAvailable(car, day.AddHours(w), day.AddHours(w + 2)))
                        continue;
                    availableInDay.Add((Start: day.AddHours(w), End: day.AddHours(w + 2)));
                    w++;
                }
            }

            availableInDay = availableInDay.Where(x => x.Start >= DateTime.Now).ToList();
            days.Add(availableInDay);
        }

        return days;
    }
}