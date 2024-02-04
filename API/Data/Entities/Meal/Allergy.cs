namespace AsparagusN.Entities;

public class Allergy
{
    public Allergy(string arabicName, string englishName)
    {
        ArabicName = arabicName;
        EnglishName = englishName;
    }

    public Allergy()
    {
    }

    public int Id { get; set; }
    public string ArabicName { get; set; }
    public string EnglishName { get; set; }
    public string PictureUrl { get; set; }
}