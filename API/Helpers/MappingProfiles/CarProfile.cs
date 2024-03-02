using AsparagusN.Data;
using AsparagusN.Data.Entities;
using AsparagusN.DTOs.CarDtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AsparagusN.Helpers.MappingProfiles
{
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            CreateMap<NewCarDto, Car>();
            CreateMap<UpdateCarDto, Car>()
                .ForMember(dest => dest.WorkingDays, opt => opt.MapFrom(c => GetCarWorkingDays(c.WorkingDays)))
                .ForAllMembers(dest => dest.Condition((src, b, member) => member != default));
            CreateMap<Car, CatInfoDto>()
                .ForMember(dest => dest.WorkingDays, opt => opt.MapFrom(src => GetCatInfoDtoWorkingDays(src.WorkingDays)));

            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.phoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber));
        }

        private List<bool> GetCatInfoDtoWorkingDays(List<CarWorkingDay> srcWorkingDays)
        {
            var result = new List<bool>();
            var days = srcWorkingDays.Select(c => c.Day).ToList();

            for (int j = 0; j <= 6; j++)
            {
                var day = (DayOfWeek)j;
                result.Add(days.Contains(day));
            }

            return result;
        }

        private List<CarWorkingDay> GetCarWorkingDays(List<bool> cWorkingDays)
        {
            var num = new List<CarWorkingDay>();

            for (int j = 0; j < cWorkingDays.Count; j++)
            {
                if (!cWorkingDays[j]) continue;
                int id = (j + 2) % 7; // Assuming you want to start from Monday, change 2 to 1 if you want to start from Sunday
                var day = (DayOfWeek)id;

                num.Add(new CarWorkingDay
                {
                    Day = day
                });
            }

            return num;
        }
    }
}
