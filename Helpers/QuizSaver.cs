using System.Text.Json;
using QuizGame.Configs;
using QuizGame.Quizes;

namespace QuizGame.Helpers
{
    public static class QuizSaver
    {
        public static void ToFile(Quiz quiz)
        {
            var path = PathsConfig.Init().PathToQuizes;
            var quizList = QuizLoader.FromFile();
            if (quizList is not null)
            {
                quizList.Add(quiz);
            }
            else
            {
                quizList = new List<Quiz>
                {
                    quiz
                };
            }
            File.Delete(path + "QuizesList.json");
            using var file = new FileStream(path + "QuizesList.json", FileMode.Create, FileAccess.Write);
            JsonSerializer.Serialize(file, quizList);
        }
        public static void ReSaveToFile(Quiz quizLocal)
        {
            var path = PathsConfig.Init().PathToQuizes;
            var list = QuizLoader.FromFile();
            var index = list.IndexOf(QuizLoader.FindQuiz($"{quizLocal.Theme} ({quizLocal.Level})", list));
            list[index] = quizLocal;
            File.Delete(path + "QuizesList.json");
            using var file = new FileStream(path + "QuizesList.json", FileMode.Create, FileAccess.Write);
            JsonSerializer.Serialize(file, list);
        }
        public static void RefreshQuizList(List<Quiz> list)
        {
            var path = PathsConfig.Init().PathToQuizes;
            File.Delete(path + "QuizesList.json");
            using var file = new FileStream(path + "QuizesList.json", FileMode.Create, FileAccess.Write);
            JsonSerializer.Serialize(file, list);
        }
    }
}
