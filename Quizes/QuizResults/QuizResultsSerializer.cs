using System.Text.Json;
using QuizGame.Configs;

namespace QuizGame.Quizes.QuizResults
{
    public class QuizResultsSerializer
    {
        protected QuizResultsSerializer()
        {
        }
        public static void CreateQuizResultsUserFile(string login)
        {
            var path = PathInit();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var file = new FileStream(path + login + ".json", FileMode.Create, FileAccess.Write);
            Serialize(ref file, new List<QuizResult>());
            file.Close();
        }
        public static void Add(string login, QuizResult result)
        {
            if (result.Scores > 0)
            {
                var path = PathInit();
                var results = LoadFromFile(path, login);
                results.Add(result);
                SaveToFile(path, login, results);
            }
        }
        private static List<QuizResult> LoadFromFile(string path, string login)
        {
            var file = new FileStream(path + login + ".json", FileMode.Open, FileAccess.Read);
            var results = Deserialize(ref file);
            file.Close();
            return results;
        }
        private static void SaveToFile(string path, string login, List<QuizResult> results)
        {
            File.Delete(path + login + ".json");
            var file = new FileStream(path + login + ".json", FileMode.Create, FileAccess.Write);
            Serialize(ref file, results);
            file.Close();
        }
        public static List<QuizResult> GetQuizResults(string login)
        {
            var path = PathInit();
            var results = LoadFromFile(path, login);
            return results;
        }
        private static string PathInit()
        {
            return PathsConfig.Init().PathToQuizResults!;
        }
        private static void Serialize(ref FileStream file, List<QuizResult> results)
        {
            JsonSerializer.SerializeAsync(file, results);
        }
        private static List<QuizResult> Deserialize(ref FileStream file)
        {
            return JsonSerializer.DeserializeAsync<List<QuizResult>>(file).AsTask().Result!;
        }
    }
}
