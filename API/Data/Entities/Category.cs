namespace AsparagusN.Entities;

public class Category
{
    public Category()
    {
    }

    public Category(string nameEn, string nameAr, string description)
    {
        NameEN = nameEn;
        NameAR = nameAr;
        Description = description;
    }

    public int Id { get; set; }
    public string NameEN { get; set; } = "";
    public string NameAR { get; set; } = "";
    public string Description { get; set; } = "";
    public List<Meal> Meals { get; set; }

}