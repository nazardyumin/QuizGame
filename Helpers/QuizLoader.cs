﻿using System.Text.Json;

public static class QuizLoader
{
    public static List<Quiz> FromFile()
    {
        string path = PathsConfig.Init().PathToQuizes;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            FileStream? helpfile = null;
            var file = new FileStream(path + "QuizesList.json", FileMode.OpenOrCreate, FileAccess.Read);
            var newfile = SerializerHelper.IfEmptyQuizesListFile(ref file, ref helpfile, path + "QuizesList.json");
            var list = JsonSerializer.DeserializeAsync<List<Quiz>>(newfile).Result;
            newfile.Close();
            return list;
        }
        else
        {
            FileStream? helpfile = null;
            var file = new FileStream(path + "QuizesList.json", FileMode.OpenOrCreate, FileAccess.Read);
            var list = JsonSerializer.DeserializeAsync<List<Quiz>>(file).Result;
            file.Close();
            return list;
        }
    }
    public static Quiz FindQuiz(string theme, List<Quiz> list)  
    {
        var quiz = list.Find((q) => $"{q.Theme} ({q.Level})" == theme);
        return quiz;
    }
    public static Quiz FindQuiz(int index)
    {
        return FromFile()[index];
    }
    public static Quiz MakeMixedQuiz()
    {
        var creator = new QuizCreator(new User());
        creator.SetTheme("Mixed Quiz");
        creator.SetLevel("Mixed");
        Random random = new();
        var quiz_list = FromFile();
        foreach (var item in quiz_list)
        {
            int i = random.Next(0, item.Questions.Count());
            creator.SetQuestion(item.Questions[i].Question);
            foreach (var answers in item.Questions[i].Answers)
            {
                creator.SetAnswer(answers.Answer, answers.IsCorrect);
            }
            creator.AddItem();
        }
        return creator.GetQuiz();
    }
}