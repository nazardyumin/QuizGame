using System.Text.Json;

public class CreateQuiz
{
    public Quiz _quiz { get; set; }
    private string? _question;
    private List<QuizAnswer> _answers;
    public CreateQuiz()
    {
        _quiz = new Quiz();
        _quiz.Questions = new List<QuizQuestion>();
        _answers = new List<QuizAnswer>();
    }
    public void SetTheme(string theme)
    {
        _quiz.Theme = theme;
    }
    public void SetLevel(string level)
    {
        _quiz.Level = level;
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
    public void SaveToQuizListFile()
    {
        string path = PathsConfig.Init().PathToQuizes;
        var quizlist = LoadQuizListFromFile(path);
        if (quizlist is not null)
        {
            quizlist.Add(_quiz);
        }
        else
        {
            quizlist = new List<Quiz>();
            quizlist.Add(_quiz);
        }
        using var file = new FileStream(path + "Quizes.json", FileMode.Open, FileAccess.Write);
        file.Position = 0;
        JsonSerializer.SerializeAsync(file, quizlist);
    }
    private List<Quiz>? LoadQuizListFromFile(string path)  //TODO добавить загрузку из файла DefaultQuizList
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        FileStream? helpfile = null;
        var file1 = new FileStream(path + "Quizes.json", FileMode.OpenOrCreate, FileAccess.Read);
        var newfile = SerializerHelper.IfEmptyQuizesFile(ref file1, ref helpfile, path + "Quizes.json");
        var list = JsonSerializer.DeserializeAsync<List<Quiz>>(newfile).Result;
        newfile.Close();
        return list;
    }
}



