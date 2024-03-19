using AsparagusN.Data.Entities.Identity;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs.PackageDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Helpers;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AsparagusN.Specifications.UserSpecifications;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard;

public class PackagingController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PackagingController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [Authorize(Roles = nameof(DashboardRoles.Packaging) + "," + nameof(Roles.Admin))]
    [HttpGet]
    public async Task<ActionResult> GetMealsForPackaging([FromQuery] string dayDate)
    {
        if (!DateTime.TryParseExact(dayDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out var parsedDayDate))
        {
            return Ok(new ApiResponse(400, "Wrong date format... it must be like yyyy-MM-dd"));
        }

        parsedDayDate = parsedDayDate.Date;

        var resultList = new List<PackageDto>();
        var spec = new UserPlanDayWithMealsAndDrinksAndAllSpecification(parsedDayDate);
        var planDays = await _unitOfWork.Repository<UserPlanDay>().ListWithSpecAsync(spec);
        var customersSpec = new CustomersSpecification(false);
        var customers = await _unitOfWork.Repository<AppUser>().ListWithSpecAsync(customersSpec);

        foreach (var day in planDays)
        {
            var packageDto = new PackageDto();
            _mapper.Map(day.UserPlan, packageDto);

            var user = customers.First(x => x.Id == day.UserPlan.AppUserId);
            packageDto.CustomerInfo = _mapper.Map<CustomerInfoDto>(user);
            var nuts = day.SelectedExtraOptions.Where(c => c.OptionType == ExtraOptionType.Nuts).ToList();
            var salads = day.SelectedExtraOptions.Where(c => c.OptionType == ExtraOptionType.Salad).ToList();

            packageDto.Meals = _mapper.Map<List<MealInfoDto>>(day.SelectedMeals);
            packageDto.Snacks = _mapper.Map<List<SnackInfoDto>>(day.SelectedSnacks);
            packageDto.Drinks = _mapper.Map<List<DrinkInfoDto>>(day.SelectedDrinks);
            packageDto.Nuts = _mapper.Map<List<NutsInfoDto>>(nuts);
            packageDto.Salads = _mapper.Map<List<SaladInfoDto>>(salads);
            packageDto.Id = day.Id;
            resultList.Add(packageDto);
        }

        resultList.Sort(new PackageDtoComparer());
        return Ok(new ApiOkResponse<List<PackageDto>>(resultList));
    }

    [Authorize(Roles = nameof(DashboardRoles.Packaging) + "," + nameof(Roles.Admin))]
    [HttpPut]
    public async Task<ActionResult> MakePrint([FromBody]List<int> dayIds)
    {
        var planDays = await _unitOfWork.Repository<UserPlanDay>().ListAllAsync();
        var ids = planDays.Select(c => c.Id).ToList();

        if (dayIds.Any(d => ids.All(c => c != d)))
            return Ok(new ApiResponse(400,"some ids not found"));

        Console.WriteLine("GGGGGG");
        planDays = planDays.Where(c => dayIds.Contains(c.Id)).ToList();

        foreach (var day in planDays)
        {
            day.IsMealsInfoPrinted = true;
            _unitOfWork.Repository<UserPlanDay>().Update(day);
        }

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200));
        return Ok(new ApiResponse(400, "Some error happened"));
    }
}