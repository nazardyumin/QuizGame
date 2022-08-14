public class QuizPlayer : Default
{
    private Quiz? _quiz;
    private QuizResult _result;
    private RatingPosition? _position;
    public QuizPlayer(User user):base(user)
    {
        _quiz = new Quiz();
        _result = new QuizResult();
        _result.AnsweredQuestions = new();
        _result.Scores = 0;
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
            listthemes.Add($"{item.Theme} ({item.Level})");
        }
        return listthemes;
    }
    public Quiz? FindQuiz(int index)
    {
        return QuizLoader.FindQuiz(index);
    }
    public Quiz MixedQuiz()
    {
        return QuizLoader.MakeMixedQuiz();
    }
    public int GetCount()
    {
        return _quiz.Questions.Count();
    }
    public string GetTheme()
    {
        return _quiz.Theme;
    }
    public string GetLevel()
    {
        return _quiz.Level;
    }
    public string GetQuestion(int index)
    {
        return _quiz.Questions[index].Question;
    }
    public List<string> GetListAnswers(int index)
    {
        List<string> list = new();
        for (int i = 0; i < 4; i++)
        {
            list.Add(GetAnswer(index, i));
        }
        return list;
    }
    private string GetAnswer(int index1, int index2)
    {
        return (_quiz.Questions[index1].Answers[index2].Answer);
    }
    public bool CheckingAnswer(int index, bool res1, bool res2, bool res3, bool res4)
    { 
        bool check1 = false;
        bool check2 = false;
        bool check3 = false;
        bool check4 = false;
        if (_quiz.Questions[index].Answers[0].IsCorrect == 1 && res1 == true) check1 = true;
        if (_quiz.Questions[index].Answers[0].IsCorrect == 0 && res1 == false) check1 = true;
        if (_quiz.Questions[index].Answers[1].IsCorrect == 1 && res2 == true) check2 = true;
        if (_quiz.Questions[index].Answers[1].IsCorrect == 0 && res2 == false) check2 = true;
        if (_quiz.Questions[index].Answers[2].IsCorrect == 1 && res3 == true) check3 = true;
        if (_quiz.Questions[index].Answers[2].IsCorrect == 0 && res3 == false) check3 = true;
        if (_quiz.Questions[index].Answers[3].IsCorrect == 1 && res4 == true) check4 = true;
        if (_quiz.Questions[index].Answers[3].IsCorrect == 0 && res4 == false) check4 = true;
        if (check1 == true && check2 == true && check3 == true && check4 == true) return true;
        else return false;
    }
    public void AddItemToQuizResult(int index,bool is_correct)
    {
        var item = new QuizQuestionResult();
        item.Question = _quiz.Questions[index].Question;
        item.IsCorrect=is_correct;
        _result.AnsweredQuestions.Add(item);
        if (is_correct) _result.Scores++;
    }
    private void AddQuizResultToUser()
    {
        _result.Theme = _quiz.Theme;
        _result.Level = _quiz.Level;
        _result.Date = $"{DateTime.Now:g}";
        var database = new UsersDataBase();
        database.LoadFromFile();
        int index=database.Users.IndexOf(database.SearchByLogin(_user.Login));
        if (database.Users[index] is not null && _user.Results is not null)
        {
            database.Users[index].Results.Add(_result);
            _user.Results.Add(_result);
        }
        else
        {
            database.Users[index].Results = new();
            database.Users[index].Results.Add(_result);
            _user.Results = new();
            _user.Results.Add(_result);
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
                    var ordered =_quiz.Top20.OrderByDescending((p) => p.Scores);
                    int i = 0;
                    foreach (var item in ordered)
                    {
                        _quiz.Top20[i] = item;
                        i++;
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
    public void SaveResults(bool is_mixed)
    {
        AddQuizResultToUser();
        PositionInit();
        if (!is_mixed)
        {
            SaveResultToTop20();
            QuizSaver.ReSaveToFile(_quiz);
        }
        SaveResultToHighscoresFile();
    }
    public List<string>? GetTop20()
    {
        List <string> list=new();
        if (_quiz.Top20 is not null )
        {
            foreach (var item in _quiz.Top20)
            {
                list.Add($"{item.Name} {item.Scores}");
            }
        }
        return list;
    }
    public string? GetHighScores()
    {
        string highscores="";
        var list=SerializerHelper.LoadHighscores();
        if (list is not null)
        {
            if (list.Count()>9)
            {
                for (int i=0;i<10; i++)
                {
                    highscores+= $"{list[i].Name} {list[i].Scores}\n";
                }
            }
            else
            {
                foreach (var item in list)
                {
                    highscores += $"{item.Name} {item.Scores}\n";
                }
            }      
        }
        return highscores;
    }
    public List<QuizResult>? GetQuizResults()
    {
        return _user.Results;
    }
    public void ResetQuizResult()
    {
        _result.Theme = "";
        _result.Level = "";
        _result.AnsweredQuestions.Clear();
        _result.Scores = 0;
    }
    public int GetScores()
    {
        return _result.Scores;
    }
    public string GetPlayerInfo()
    {
        int quizes_passed = 0;
        int total_scores = 0;
        var database = new UsersDataBase();
        database.LoadFromFile();
        int index = database.Users.IndexOf(database.SearchByLogin(_user.Login));
        if (database.Users[index].Results is not null)
        {
            quizes_passed = database.Users[index].Results.Count();
            foreach (var item in database.Users[index].Results)
            {
                total_scores += item.Scores;
            }
        }
        return $"Quizes passed: {quizes_passed} | Total Scores: {total_scores} | ";
    }
}