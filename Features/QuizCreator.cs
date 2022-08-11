using System.Text.Json;

public class QuizCreator : QuizEditor
{
    private string? _question;
    private List<QuizAnswer> _answers;
    public QuizCreator(User user):base(user)
    {
        _quiz = new Quiz();
        _quiz.Questions = new List<QuizQuestion>();
        _answers = new List<QuizAnswer>();
    }
    public Quiz? FindQuiz(string theme)
    {
        return QuizLoader.FindQuiz(theme, QuizLoader.FromFile());
    }
    public void SetQuestion(string question)
    {
        _question = question;
    }
    public void SetAnswer(string answer, int iscorrect)
    {
        _answers.Add(new QuizAnswer { Answer = answer, IsCorrect = iscorrect });
    }
    public void AddItem()
    {
        var temp = new QuizQuestion();
        temp.Answers = new List<QuizAnswer>();
        foreach (var item in _answers)
        {
            temp.Answers.Add(item);
        }
        temp.Question = _question;
        _quiz.Questions.Add(temp);
        _question = "";
        _answers.Clear();
    }
    public void Clear()
    {
        _quiz.Questions.Clear();
    }
    public Quiz GetQuiz()
    {
        return _quiz;
    }
}