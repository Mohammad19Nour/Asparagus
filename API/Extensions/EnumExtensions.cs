using AsparagusN.Enums;

namespace AsparagusN.Extensions;

public static class EnumExtensions
{
    public static string GetName(this Gender g)
    {
        return g == Gender.Female ? "female" : "male";
    }
}