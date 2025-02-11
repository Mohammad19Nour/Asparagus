﻿using System.Linq.Expressions;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications.UserSpecifications;

public class UserPlanDayWithSnacksOnlySpecification : BaseSpecification<UserPlanDay>
{
    public UserPlanDayWithSnacksOnlySpecification(int userId, int dayId) 
        : base(x=>x.UserPlan.AppUserId == userId && x.Id == dayId)
    {
        
        AddInclude(x=>x.Include(y=>y.UserPlan));
        AddInclude(x=>x.Include(y=>y.SelectedSnacks));
    }
}