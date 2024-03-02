namespace AsparagusN.Enums
{
    public enum DashboardRoles
    {
        Statistics,
        Users,
        SubMeals,
        Meals,
        MealPlans,
        DeliveryZones,
        Drivers,
        Export,
        Packaging,
        Branches,
        Slider,
        SplashScreen,
        Extra,
        Drinks,
        Cashiers,
        Coupons,
        Notifications,
        Orders,
        MealPoints,
        Questions,
        PlanOrders
    }
    public static class DashboardRolesExtensions
    {
        public static string GetName(this DashboardRoles role)
        {
            return Enum.GetName(typeof(DashboardRoles), role);
        }
    }
}