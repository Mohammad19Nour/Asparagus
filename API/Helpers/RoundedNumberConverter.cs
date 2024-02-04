using System.Text.Json;
using System.Text.Json.Serialization;

namespace AsparagusN.Helpers;

public class RoundedNumberConverter : JsonConverter<decimal>
{
     private readonly int decimalPlaces;

    public RoundedNumberConverter(int decimalPlaces)
    {
        this.decimalPlaces = decimalPlaces;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(decimal) || objectType == typeof(double) || objectType == typeof(float) ||objectType == typeof(int) ;
    }

    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            var originalValue = reader.GetDecimal();
            decimal roundedValue = Math.Round(originalValue, decimalPlaces);
            return roundedValue;
        }
        throw new JsonException($"Unable to convert JSON token type '{reader.TokenType}' to double.");
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(Math.Round(value,decimalPlaces));
    }

   
}