﻿using System.Runtime.Serialization;

namespace AsparagusN.Enums;

public enum MealPlanType
{
    [EnumMember(Value = "Loss Weight")]
    LossWeight,
    [EnumMember(Value = "Maintain Weight")]
    MaintainWeight,
    [EnumMember(Value = "Future Leader")]
    FutureLeader,
    [EnumMember(Value = "Custom meal plan")]
    CustomMealPlan
}