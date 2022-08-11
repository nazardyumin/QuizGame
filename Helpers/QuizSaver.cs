using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

public static class QuizSaver
{
    public static void ToFile(Quiz quiz)
    {
        string path = PathsConfig.Init().PathToQuizes;
        var quizlist = QuizLoader.FromFile(path);
        if (quizlist is not null)
        {
            quizlist.Add(quiz);
        }
        else
        {
            quizlist = new List<Quiz>();
            quizlist.Add(quiz);
        }
        File.Delete(path + "Quizes.json");
        using var file = new FileStream(path + "Quizes.json", FileMode.Create, FileAccess.Write);
        JsonSerializer.SerializeAsync(file, quizlist);
    }
}
