public class QuizAnsweredItem
{
    public QuizQuestion? AnsweredQuestion { get; set; }
    public bool IsCorrect { get; set; }
    public QuizAnsweredItem(QuizQuestion question, bool is_correct)
    {
        AnsweredQuestion=question;  
        IsCorrect=is_correct;   
    }
}