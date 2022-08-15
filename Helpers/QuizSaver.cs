using System.Text.Json;

public static class QuizSaver
{
    public static void ToFile(Quiz quiz)
    {
        string path = PathsConfig.Init().PathToQuizes;
        var quizlist = QuizLoader.FromFile();
        if (quizlist is not null)
        {
            quizlist.Add(quiz);
        }
        else
        {
            quizlist = new List<Quiz>();
            quizlist.Add(quiz);
        }
        File.Delete(path + "QuizesList.json");
        using var file = new FileStream(path + "QuizesList.json", FileMode.Create, FileAccess.Write);
        JsonSerializer.SerializeAsync(file, quizlist);
    }
    public static void ReSaveToFile(Quiz quiz_local)
    {
        string path = PathsConfig.Init().PathToQuizes;
        var list = QuizLoader.FromFile();
        var index=list.IndexOf(QuizLoader.FindQuiz($"{quiz_local.Theme} ({ quiz_local.Level})",list));
        list[index] = quiz_local;
        File.Delete(path + "QuizesList.json");
        using var file = new FileStream(path + "QuizesList.json", FileMode.Create, FileAccess.Write);
        JsonSerializer.SerializeAsync(file, list);
    }
    public static void RefreshQuizList(List<Quiz>list)
    {
        string path = PathsConfig.Init().PathToQuizes;
        File.Delete(path + "QuizesList.json");
        using var file = new FileStream(path + "QuizesList.json", FileMode.Create, FileAccess.Write);
        JsonSerializer.SerializeAsync(file, list);
    }
}