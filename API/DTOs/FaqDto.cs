namespace AsparagusN.DTOs;

public class FaqDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<QuestionDto> Questions;
}