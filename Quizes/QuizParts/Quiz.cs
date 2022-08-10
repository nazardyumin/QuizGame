public class Quiz
{
    public string? Theme { get; set; }
    public List<QuizQuestion>? Questions { get; set; }
    public string? Level { get; set; }
    public List<RatingPosition>? Top20 { get; set; }
}