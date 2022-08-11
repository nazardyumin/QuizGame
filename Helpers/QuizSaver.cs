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
        var quiz_from_file = QuizLoader.FindQuiz(quiz_local.Theme,list);
        if (quiz_from_file.Top20 is not null)
        {
            quiz_from_file.Top20.Clear();
            foreach (var item in quiz_local.Top20)
            {
                quiz_from_file.Top20.Add(item);
            }
        }
        else
        {
            quiz_from_file.Top20 = new List<RatingPosition>();
            foreach (var item in quiz_local.Top20)
            {
                quiz_from_file.Top20.Add(item);
            }
        }
        File.Delete(path + "QuizesList.json");
        using var file = new FileStream(path + "QuizesList.json", FileMode.Create, FileAccess.Write);
        JsonSerializer.SerializeAsync(file, list);
    }
}