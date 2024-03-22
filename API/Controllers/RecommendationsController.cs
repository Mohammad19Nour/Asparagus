using AsparagusN.Data.Entities.Identity;
using AsparagusN.DTOs;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace AsparagusN.Controllers;

public class RecommendationsController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlanRecommendationService _recommendationService;

    public RecommendationsController(IUnitOfWork unitOfWork,IPlanRecommendationService recommendationService)
    {
        _unitOfWork = unitOfWork;
        _recommendationService = recommendationService;
    }
[Authorize]
    [HttpGet]
    public async Task<ActionResult> GetRecommendation(double weight, double height, ActivityLevel activityLevel, PlanTypeEnum targetPlan)
    {
        var email = User.GetEmail();
        var user = await _unitOfWork.Repository<AppUser>().GetQueryable()
            .Where(c => c.Email.ToLower() == email).FirstOrDefaultAsync();

        if (user == null)
            return Ok(new ApiResponse(404, "User not found"));
        
        var input = new RecommendationInput
        {
            HeightCm = height,
            WeightKg = weight,
            Age = user.GetAge(),
            ActivityLevel = activityLevel,
            Gender = user.Gender,
            TargetPlan = targetPlan
        };
        var result = _recommendationService.GetRecommendedPlan(input);
        return Ok(new ApiOkResponse<string>(result.GetDisplayName()));
    }
}