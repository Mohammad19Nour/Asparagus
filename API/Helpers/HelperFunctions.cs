namespace AsparagusN.Helpers;

public static class HelperFunctions
{
    public static List<DateTime> getDatesOfCurrentWeek()
    {
        var result = new List<DateTime>();
        var startDay = DateTime.Now;

        while (startDay.DayOfWeek != DayOfWeek.Saturday)
        {
            startDay = startDay.AddDays(-1);
        }

        result.Add(startDay);

        while (startDay.DayOfWeek != DayOfWeek.Friday)
        {
            startDay = startDay.AddDays(1);
            result.Add(startDay);
        }

        return result;
    }
}