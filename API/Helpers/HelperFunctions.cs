namespace AsparagusN.Helpers;

public static class HelperFunctions
{
    public static DateTime WeekStartDay()
    {
        var todayDay = DateTime.Now.DayOfWeek;
        var startDay = DateTime.Now.Date;
        var add = -1;
        if (todayDay is DayOfWeek.Friday or DayOfWeek.Thursday)
            add = 1;

        while (startDay.DayOfWeek != DayOfWeek.Saturday)
        {
            startDay = startDay.AddDays(add);
        }

        return startDay;
    }

    public static DateTime WeekEndDay()
    {
        Console.WriteLine(WeekStartDay());
        Console.WriteLine(WeekStartDay().AddDays(6));
        return WeekStartDay().AddDays(6);
    }

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