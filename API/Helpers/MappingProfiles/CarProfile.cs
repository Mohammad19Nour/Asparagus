using AsparagusN.Data.Entities;
using AsparagusN.DTOs.CarDtos;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class CarProfile : Profile
{
    public CarProfile()
    {
        CreateMap<NewCarDto, Car>();
        CreateMap<UpdateCarDto, Car>()
            .ForMember(dest => dest.WorkingDays, opt => opt.MapFrom(c => getEnumDays(c.WorkingDays)))
            .ForAllMembers(dest => dest.Condition((src, b, member) => member != default));
    }

    private List<CarWorkingDay> getEnumDays(List<int> cWorkingDays)
    {
        var num = new List<CarWorkingDay>(7);

        for (int j = 0; j < 7; j++)
        {
            if (cWorkingDays[j] != 1) continue;
            int id = (j + 2) % 7;
            var day = (DayOfWeek)id;

            num.Add(new CarWorkingDay
            {
                Day = day
            });
        }

        return num;
    }
}