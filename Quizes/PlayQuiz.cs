using System.Text.Json;

public class PlayQuiz
{
    protected Quiz Quiz;
    protected User Player;
    public QuizResult Result;
    public RatingPosition? Pos;
    private (string top20, string highscores, string quizes) Paths;
    public PlayQuiz(User player)
    {
        Player = player;
        Paths = PathInit();
        Result = new QuizResult(); //testing!!!!!!!!!!!!
    }
    public void FindQuizAndInit(string theme)
    {
        var quizlist = LoadQuizListFromFile(Paths.quizes);
        Quiz = quizlist.Find((q) => q.Theme == theme);
    }
    public void SaveResults()
    {
        PositionInit();
        SaveResultToTop20File(Paths.top20);
        SaveResultToHighscoresFile(Paths.highscores);
    }
    public List<string> GetAllQuizThemes()
    {
        var quizlist = LoadQuizListFromFile(Paths.quizes);
        var listthemes = new List<string>();
        foreach (var item in quizlist)
        {
            listthemes.Add(item.Theme);
        }
        return listthemes;
    }
    private List<Quiz>? LoadQuizListFromFile(string path)
    {
        var file = new FileStream(path + "Quizes.json", FileMode.Open, FileAccess.Read);
        var list = JsonSerializer.DeserializeAsync<List<Quiz>>(file).Result;
        file.Close();
        return list;
    }
    private void SaveResultToTop20File(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        FileStream? helpfile = null;
        var file1 = new FileStream(path + Quiz.Theme + "Top20.json", FileMode.OpenOrCreate, FileAccess.Read);
        var newfile = SerializerHelper.IfEmptyRatingPositionFile(ref file1, ref helpfile, path + Quiz.Theme + "Top20.json");
        var Top20 = JsonSerializer.DeserializeAsync<List<RatingPosition>>(newfile).Result;
        newfile.Close();
        if (Top20 is not null)
        {
            if (Top20.Contains(Top20.Find((i) => i.Name == Pos.Name)))
            {
                var item = Top20.Find((i) => i.Name == Pos.Name);
                if (item.Scores < Pos.Scores) item.Scores = Pos.Scores;
            }
            else
            {
                Top20.Add(Pos);

            }
            var orderedTop20 = Top20.OrderByDescending((r) => r.Scores);
            using var file2 = new FileStream(path + Quiz.Theme + "Top20.json", FileMode.Open, FileAccess.Write);
            file2.Position = 0;
            JsonSerializer.SerializeAsync(file2, orderedTop20);
        }
        else
        {
            Top20 = new List<RatingPosition>();
            Top20.Add(Pos);
            using var file3 = new FileStream(path + Quiz.Theme + "Top20.json", FileMode.Open, FileAccess.Write);
            file3.Position = 0;
            JsonSerializer.SerializeAsync(file3, Top20);
        }
    }
    private void SaveResultToHighscoresFile(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        FileStream? helpfile = null;
        var hsfile1 = new FileStream(path + "Highscores.json", FileMode.OpenOrCreate, FileAccess.Read);
        var newfile = SerializerHelper.IfEmptyRatingPositionFile(ref hsfile1, ref helpfile, path + "Highscores.json");
        var Highscores = JsonSerializer.DeserializeAsync<List<RatingPosition>>(newfile).Result;
        newfile.Close();
        if (Highscores is not null)
        {
            if (Highscores.Contains(Highscores.Find((i) => i.Name == Pos.Name)))
            {
                var item = Highscores.Find((i) => i.Name == Pos.Name);
                item.Scores += Pos.Scores;
            }
            else
            {
                Highscores.Add(Pos);
            }
            var orderedHighScores = Highscores.OrderByDescending((r) => r.Scores);
            using var hsfile2 = new FileStream(path + "Highscores.json", FileMode.Open, FileAccess.Write);
            hsfile2.Position = 0;
            JsonSerializer.SerializeAsync(hsfile2, orderedHighScores);
        }
        else
        {
            Highscores = new List<RatingPosition>();
            Highscores.Add(Pos);
            using var hsfile3 = new FileStream(path + "Highscores.json", FileMode.Open, FileAccess.Write);
            hsfile3.Position = 0;
            JsonSerializer.SerializeAsync(hsfile3, Highscores);
        }
    }
    private void PositionInit()
    {
        var Pos = new RatingPosition();
        Pos.Scores = Result.Scores;
        Pos.Name = Player.FirstName + " " + Player.LastName;
    }
    private (string top20, string highscores, string quizes) PathInit()
    {
        var paths = PathsConfig.Init();
        return (paths.PathToTop20, paths.PathToHighscores, paths.PathToQuizes);
    }
}

