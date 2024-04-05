using Newtonsoft.Json.Converters;

namespace AsparagusN.Helpers;

public class DateTimeConverter : IsoDateTimeConverter
{
    public DateTimeConverter()
    {
        base.DateTimeFormat = "yyyy-MM-dd HH:mm";
    }
}