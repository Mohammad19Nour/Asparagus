using System.Runtime.Serialization;

namespace AsparagusN.Enums;

public enum IngredientType
{
    [EnumMember(Value = "Protein")]
    Protein,
    [EnumMember(Value = "Carb")]
    Carb,
    [EnumMember(Value = "Soup")]
    Soup 
    
}