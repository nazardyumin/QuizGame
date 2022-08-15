public class QuizEditor : Default
{
    protected Quiz? _quiz { get; set; }
    public QuizEditor(User user):base(user)
    {    
    }
    public Quiz? FindQuiz(int index)
    {
        return QuizLoader.FindQuiz(index);
    }
    public void QuizInit(Quiz quiz)
    {
        _quiz = quiz;
    }
    public void SetTheme(string theme)
    {
        _quiz.Theme = theme;
    }
    public void SetLevel(string level)
    {
        _quiz.Level = level;
    }
    public void EditQuestion(string question, int index)
    {
        _quiz.Questions[index].Question = question;
    }
    public void EditAnswer(string answer, int iscorrect, int index1, int index2)
    {
        _quiz.Questions[index1].Answers[index2].Answer = answer;
        _quiz.Questions[index1].Answers[index2].IsCorrect = iscorrect;
    }
    public int GetCount()
    {
        return _quiz.Questions.Count();
    }
    public string GetQuestion(int index)
    {
        if (_quiz.Questions.Count()>0)
        {
            return _quiz.Questions[index].Question;
        }
        else
        {
            return "";
        }
    }
    public List<(string, int)> GetAnswers(int index)
    {
        var list = new List<(string, int)>();
        if (_quiz.Questions.Count() > 0)
        {
            foreach (var item in _quiz.Questions[index].Answers)
            {
                list.Add((item.Answer, item.IsCorrect));
            }
        }
        else
        {
            for(int i=0;i<4;i++)
            {
                list.Add(("", 0));
            }
        }
        return list;
    }
    public (string, int) GetAnswer(int index1, int index2)
    {
        return (_quiz.Questions[index1].Answers[index2].Answer, _quiz.Questions[index1].Answers[index2].IsCorrect);
    }
    public List<string> GetAllQuizThemes()
    {
        var quizlist = QuizLoader.FromFile();
        var listthemes = new List<string>();
        foreach (var item in quizlist)
        {
            listthemes.Add($"{item.Theme} ({item.Level})");
        }
        return listthemes;
    }
    public string GetTheme()
    {
        return _quiz.Theme;
    }
    public string GetLevel()
    {
        return _quiz.Level;
    }
    public int GetLevelInt()
    {
        if (_quiz.Level == "Easy") return 0;
        else if (_quiz.Level == "Normal") return 1;
        else return 2;
    }
    public void DeleteItem(int index)
    {
        _quiz.Questions.Remove(_quiz.Questions[index]);
    }
}