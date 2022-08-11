public class QuizPlayer : Default
{
    private Quiz? _quiz;
    private QuizResult _result;
    private RatingPosition? _position;
    public QuizPlayer(User user):base(user)
    {
        _result = new QuizResult();
    }
    public void SetQuiz(Quiz quiz)
    {
        _quiz = quiz;
    }
    public List<string> GetAllQuizThemes()
    {
        var quizlist = QuizLoader.FromFile();
        var listthemes = new List<string>();
        foreach (var item in quizlist)
        {
            listthemes.Add(item.Theme);
        }
        return listthemes;
    }
    public Quiz FindQuiz(string theme)
    {
        return QuizLoader.FindQuiz(theme, QuizLoader.FromFile());
    }
    public Quiz MakeMixedQuiz()
    {
        //прописать механику получения квиза со случайными вопросами из всех квизов
        return null;
    }
    public bool CheckingAnswer(int index, int item1, int item2, int item3, int item4)
    {
        bool check1 = false;
        bool check2 = false;
        bool check3 = false;
        bool check4 = false;
        if (_quiz.Questions[index].Answers[0].IsCorrect == item1) check1 = true;
        if (_quiz.Questions[index].Answers[1].IsCorrect == item2) check2 = true;
        if (_quiz.Questions[index].Answers[2].IsCorrect == item3) check3 = true;
        if (_quiz.Questions[index].Answers[3].IsCorrect == item4) check4 = true;
        if (check1 == true && check2 == true && check3 == true && check4 == true) return true;
        else return false;
    }
    public void AddItemToQuizResult(QuizQuestion question, bool is_correct)
    {
        var item = new QuizAnsweredItem(question, is_correct);
        _result.AnsweredQuestions.Add(item);
        if (is_correct) _result.Scores++;
    }
    private void AddQuizResultToUser()
    {
        _result.Theme = _quiz.Theme;
        _result.Level = _quiz.Level;
        var database = new UsersDataBase();
        database.LoadFromFile();
        var user = database.SearchByLogin(_user.Login);
        if (_user.Results is not null)
        {
            _user.Results.Add(_result);
            user.Results.Add(_result);
        }
        else
        {
            _user.Results = new();
            _user.Results.Add(_result);
            user.Results = new();
            user.Results.Add(_result);
        }
        database.SaveToFile();
    }
    private void PositionInit()
    {
        _position = new RatingPosition();
        _position.Scores = _result.Scores;
        _position.Name = _user.FirstName + " " + _user.LastName;
    }
    private void SaveResultToTop20()
    {
        if (_position.Scores > 0)
        {
            if (_quiz.Top20 is not null)
            {
                if (_quiz.Top20.Contains(_quiz.Top20.Find((i) => i.Name == _position.Name)))
                {
                    var item = _quiz.Top20.Find((i) => i.Name == _position.Name);
                    if (item.Scores < _position.Scores) item.Scores = _position.Scores;
                }
                else
                {
                    _quiz.Top20.Add(_position);
                    var ordered = (List<RatingPosition>)_quiz.Top20.OrderByDescending((p) => p.Scores);
                    _quiz.Top20.Clear();
                    foreach (var item in ordered)
                    {
                        _quiz.Top20.Add(item);
                    }
                }
            }
            else
            {
                _quiz.Top20 = new List<RatingPosition>();
                _quiz.Top20.Add(_position);
            }
        }
    }
    private void SaveResultToHighscoresFile()
    {
        if (_position.Scores > 0)
        {
            SerializerHelper.SaveHighscores(_position);
        }
    }
    public void SaveResults()
    {
        AddQuizResultToUser();
        PositionInit();
        SaveResultToTop20();
        QuizSaver.ReSaveToFile(_quiz);
        SaveResultToHighscoresFile();
    }
    public List<RatingPosition>? GetTop20()
    {
        return _quiz.Top20;
    }
    public List<RatingPosition>? GetHighScores()
    {
        return SerializerHelper.LoadHighscores();
    }
    public List<QuizResult>? GetQuizResults()
    {
        return _user.Results;
    }
}