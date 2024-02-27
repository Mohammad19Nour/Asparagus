namespace AsparagusN.Data.Entities;

public class FAQ
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int? ParentFAQId { get; set; }
    public FAQ? ParentFAQ { get; set; }
    public List<FAQ> FAQChildern { get; set; } = new List<FAQ>();
}