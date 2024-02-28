﻿namespace AsparagusN.Data.Entities;

public class Car
{
    public int Id { get; set; }
    public string Name { get; set; } = "Car 1";
    public bool IsAvailable { get; set; } = true;
    public List<Booking> Bookings { get; set; } = new List<Booking>();
    public TimeSpan WorkingStartHour { get; set; } = TimeSpan.FromHours(8); // 8 am
    public TimeSpan WorkingEndHour { get; set; } = TimeSpan.FromHours(17); // 5 pm
    public List<CarWorkingDay> WorkingDays { get; set; } = new List<CarWorkingDay>();
}