public class QuizResult
{
    public List<QuizAnsweredItem>? AnsweredQuestions { get; set; }
    public int Scores { get; set; }
    public QuizResult()
    {
        Scores = 0;
    }
}