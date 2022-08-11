public class PlayQuiz
{
    private Quiz _quiz;
    private User _player;
    private QuizResult _result;
    private RatingPosition _position;
    private (string highscores, string quizes) _paths;
    public PlayQuiz(User player)
    {
        _player = player;
        _paths = PathInit();
        _result = new QuizResult(); //testing!!!!!!!!!!!!
    }
    public void SetQuiz(Quiz quiz)
    {
        _quiz = quiz;
    }
    public Quiz FindQuiz(string theme)
    {
        return QuizLoader.FindQuiz(theme, QuizLoader.FromFile(_paths.quizes));
    }
    public Quiz MakeMixedQuiz()
    {
        //прописать механику получения квиза со случайными вопросам из всех квизов
        return null;
    }
    public List<RatingPosition>? GetTop20()
    {
        return _quiz.Top20;
    }
    public void SaveResults()
    {
        PositionInit();
        SaveResultToTop20();
        SaveResultToHighscoresFile(_paths.highscores);
    }
    public List<string> GetAllQuizThemes()
    {
        var quizlist = QuizLoader.FromFile(_paths.quizes);
        var listthemes = new List<string>();
        foreach (var item in quizlist)
        {
            listthemes.Add(item.Theme);
        }
        return listthemes;
    }
    private void SaveResultToTop20()
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
                _quiz.Top20 = (List<RatingPosition>)_quiz.Top20.OrderByDescending((p) => p.Scores);
            }
        }
        else
        {
            _quiz.Top20 = new List<RatingPosition>();
            _quiz.Top20.Add(_position);
        }
    }
    private void SaveResultToHighscoresFile(string path)
    {
        SerializerHelper.SaveHighscores(path, _position);
    }
    private void PositionInit()
    {
        _position = new RatingPosition();
        _position.Scores = _result.Scores;
        _position.Name = _player.FirstName + " " + _player.LastName;
    }
    private (string highscores, string quizes) PathInit()
    {
        var paths = PathsConfig.Init();
        return (paths.PathToHighscores, paths.PathToQuizes);
    }
}

