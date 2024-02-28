using Newtonsoft.Json;

namespace AsparagusN.Helpers;

public class TimeSpanConverter : JsonConverter<TimeSpan>
{
    public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
    {
        writer.WriteValue(value); // Serialize TimeSpan to string in the format "hh:mm:ss"
    }

    public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.Value == null)
            return default(TimeSpan); ; // Return default TimeSpan if the value is null
        
        if (TimeSpan.TryParseExact(reader.Value.ToString(), @"hh\:mm\:ss", null, out TimeSpan result))
            return result; // Successfully parsed TimeSpan
        else
            throw new JsonSerializationException("Invalid TimeSpan format.. format should be hh:mm:ss"); // Handle parsing error
    }
}