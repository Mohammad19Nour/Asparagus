using System.Linq.Expressions;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications.UserSpecifications;

public class UserSelectedPlanDayWithExtrasOnlySpecification : BaseSpecification<UserPlanDay>
{
    public UserSelectedPlanDayWithExtrasOnlySpecification(int userId, int dayId)
        : base(x => x.Id == dayId && x.UserPlan.AppUserId == userId)
    {
        AddInclude(x => x.Include(y => y.UserPlan));
        AddInclude(x => x.Include(y => y.SelectedExtraOptions));
    }

    public UserSelectedPlanDayWithExtrasOnlySpecification(int userId, int dayId, ExtraOptionType extraType)
        : base(x => x.Id == dayId && x.UserPlan.AppUserId == userId &&
                    x.SelectedExtraOptions.Any(y => y.OptionType == extraType))
    {
        AddInclude(x => x.Include(y => y.UserPlan));
        AddInclude(x => x.Include(y => y.SelectedExtraOptions));
    }
}