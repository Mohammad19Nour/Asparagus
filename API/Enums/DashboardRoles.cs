using System.Runtime.Serialization;

namespace AsparagusN.Enums
{
    public enum DashboardRoles
    {
        [EnumMember(Value = "Dashboard")]
        Dashboard = 0,
        [EnumMember(Value = "Users")]
        Users = 1,
        [EnumMember(Value = "Sub Meal")]
        SubMeal = 2,
        [EnumMember(Value = "Category")]
        Category = 3,
        [EnumMember(Value = "Menu")]
        Menu = 4,
        [EnumMember(Value = "Meal Plan")]
        MealPlan = 5,
        [EnumMember(Value = "Delivery Zone")]
        DeliveryZone = 6,
        [EnumMember(Value = "Drivers")]
        Drivers = 7,
        [EnumMember(Value = "Export")]
        Export = 8,
        [EnumMember(Value = "Packaging")]
        Packaging = 9,
        [EnumMember(Value = "Branchs")]
        Branches = 10,
        [EnumMember(Value = "Slider")]
        Slider = 11,
        [EnumMember(Value = "Splash")]
        Splash = 12,
        [EnumMember(Value = "Extra")]
        Extra = 13,
        [EnumMember(Value = "Drink")]
        Drink = 14,
        [EnumMember(Value = "Cashier")]
        Cashier = 15,
        [EnumMember(Value = "Coupon Code")]
        CouponCode = 16,
        [EnumMember(Value = "Notification")]
        Notification = 17,
        [EnumMember(Value = "Order")]
        Order = 18,
        [EnumMember(Value = "Meal Point")]
        MealPoint = 19,
        [EnumMember(Value = "User Question")]
        UserQuestion = 20,
        [EnumMember(Value = "Gift")]
        Gift = 21,
        [EnumMember(Value = "Car")]
        Car = 22,
        [EnumMember(Value = "Role")]
        Role = 23,
        [EnumMember(Value = "BookingCar")]
        BookingCar = 24,
        [EnumMember(Value = "Meal Plan Order")]
        MealPlanOrder = 25
    }
    public static class DashboardRolesExtensions
    {
        public static string GetName(this DashboardRoles role)
        {
            return Enum.GetName(typeof(DashboardRoles), role);
        }
    }
}