public class QuizResult
{
    public string? Theme { get; set; }
    public string? Level { get; set; }
    public List<QuizAnsweredItem> AnsweredQuestions { get; set; }
    public int Scores { get; set; }
    public QuizResult()
    {
        AnsweredQuestions = new();
        Scores = 0;
    }
}