using System.Text.Json;
using System.Text.Json.Serialization;

namespace AsparagusN.Helpers.MappingProfiles;

public class EnumConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum;
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}