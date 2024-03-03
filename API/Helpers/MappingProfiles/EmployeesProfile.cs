using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.DTOs.EmployeeDtos;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class EmployeesProfile : Profile
{
    public EmployeesProfile()
    {
        CreateMap<Employee, EmployeeDto>();
        CreateMap<AppUser, EmployeeDto>();
        CreateMap<NewEmployeeDto, Employee>();
        CreateMap<UpdateEmployeeDto, Employee>().ForAllMembers(opt =>
            opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}