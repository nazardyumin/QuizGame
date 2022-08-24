using System.Text.Json;
using QuizGame.Configs;
using QuizGame.Helpers;

namespace QuizGame.Quizes.QuizResults
{
    public class QuizResultsDataBase
    {
        private Dictionary<string, List<QuizResult>>? quizResults;
        private readonly string path;
        public QuizResultsDataBase()
        {
            path = PathInit();
        }
        public void Add(string login, QuizResult result)
        {
            if (result.Scores > 0)
            {
                LoadFromFile();
                if (quizResults!.Count > 0)
                {
                    if (quizResults.ContainsKey(login))
                    {
                        quizResults[login].Add(result);
                    }
                    else
                    {
                        quizResults.Add(login, new List<QuizResult> { result });
                    }
                }
                else
                {
                    quizResults.Add(login, new List<QuizResult> { result });
                }
                SaveToFile();
            }       
        }
        private void SaveToFile()
        {
            File.Delete(path + "QuizResults.json");
            using var file = new FileStream(path + "QuizResults.json", FileMode.Create, FileAccess.Write);
            Serialize(file, quizResults!);
        }
        private void LoadFromFile()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                FileStream? helpfile = null;
                var file = new FileStream(path + "QuizResults.json", FileMode.OpenOrCreate, FileAccess.Read);
                var newfile = SerializerHelper.IfEmptyQuizResultsFile(ref file, ref helpfile!, path + "QuizResults.json");
                var results = Deserialize(newfile);
                newfile.Close();
                quizResults = results!;
            }
            else
            {
                var file = new FileStream(path + "QuizResults.json", FileMode.Open, FileAccess.Read);
                var results = Deserialize(file);
                file.Close();
                quizResults = results!;
            }
        }
        private string PathInit()
        {
            return PathsConfig.Init().PathToQuizResults!;
        }
        private Dictionary<string, List<QuizResult>> Deserialize(FileStream file)
        {
            return JsonSerializer.DeserializeAsync<Dictionary<string, List<QuizResult>>>(file).AsTask().Result!;
        }
        private void Serialize(FileStream file, Dictionary<string, List<QuizResult>> results)
        {
            JsonSerializer.SerializeAsync(file, results);
        }
        public List<QuizResult>? GetQuizResults(string login)
        {
            LoadFromFile();
            if (quizResults!.ContainsKey(login))
            {
                return quizResults![login];
            }
            else
            {
                return new List<QuizResult>();
            }
        }
    }
}
