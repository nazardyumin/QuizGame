using System.Text.Json;
using QuizGame.Configs;
using QuizGame.Quizes;
using QuizGame.Quizes.QuizResults;
using QuizGame.Users;

namespace QuizGame.Helpers
{
    public static class SerializerHelper
    {
        public static FileStream IfEmptyRatingPositionFile(ref FileStream file, ref FileStream helpfile, string path)
        {
            if (file.Length == 0)
            {
                file.Close();
                var testrecord = new List<RatingPosition>();
                var tempfile = new FileStream(path, FileMode.Open, FileAccess.Write);
                JsonSerializer.SerializeAsync(tempfile, testrecord);
                tempfile.Close();
                helpfile = new FileStream(path, FileMode.Open, FileAccess.Read);
                return helpfile;
            }
            return file;
        }
        public static FileStream IfEmptyUsersListFile(ref FileStream file, ref FileStream helpfile, string path)
        {
            if (file.Length == 0)
            {
                file.Close();
                var newfile = new FileStream("DefaultUsersList.json", FileMode.Open, FileAccess.Read);
                var defaultusers = DeserializeUsers(newfile);
                newfile.Close();
                var tempfile = new FileStream(path, FileMode.Open, FileAccess.Write);
                JsonSerializer.SerializeAsync(tempfile, defaultusers);
                tempfile.Close();
                helpfile = new FileStream(path, FileMode.Open, FileAccess.Read);
                return helpfile;
            }
            return file;
        }
        public static FileStream IfEmptyQuizesListFile(ref FileStream file, ref FileStream helpfile, string path)
        {
            if (file.Length == 0)
            {
                file.Close();
                var newfile = new FileStream("DefaultQuizesList.json", FileMode.Open, FileAccess.Read);
                var defaultquizes = DeserializeQuizes(newfile);
                newfile.Close();
                foreach (var item in defaultquizes)
                {
                    Top20Serializer.CreateTop20File($"{item.Theme} ({item.Level})");
                }
                var tempfile = new FileStream(path, FileMode.Open, FileAccess.Write);
                JsonSerializer.SerializeAsync(tempfile, defaultquizes);
                tempfile.Close();
                helpfile = new FileStream(path, FileMode.Open, FileAccess.Read);
                return helpfile;
            }
            return file;
        }
        public static void SaveHighscores(RatingPosition position)
        {
            var path = PathsConfig.Init().PathToHighscores;
            var highscores = LoadHighscores();
            if (highscores is not null)
            {
                if (highscores.Find((i) => i.Login == position.Login) is not null)
                {
                    var item = highscores.Find((i) => i.Login == position.Login);
                    item!.Scores += position.Scores;
                }
                else
                {
                    highscores.Add(position);
                }
                var orderedHighScores = highscores.OrderByDescending((r) => r.Scores);
                File.Delete(path + "Highscores.json");
                using var hsfile2 = new FileStream(path + "Highscores.json", FileMode.Create, FileAccess.Write);
                JsonSerializer.SerializeAsync(hsfile2, orderedHighScores);
            }
            else
            {
                highscores = new List<RatingPosition>
                {
                    position
                };
                File.Delete(path + "Highscores.json");
                using var hsfile3 = new FileStream(path + "Highscores.json", FileMode.Create, FileAccess.Write);
                JsonSerializer.SerializeAsync(hsfile3, highscores);
            }
        }
        public static List<RatingPosition>? LoadHighscores()
        {
            var path = PathsConfig.Init().PathToHighscores;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path!);
            }
            FileStream? helpfile = null;
            var hsfile1 = new FileStream(path + "Highscores.json", FileMode.OpenOrCreate, FileAccess.Read);
            var newfile = IfEmptyRatingPositionFile(ref hsfile1, ref helpfile!, path + "Highscores.json");
            var highscores = DeserializeRatings(newfile);
            newfile.Close();
            return highscores;
        }
        private static List<RatingPosition> DeserializeRatings(FileStream file)
        {
            return JsonSerializer.DeserializeAsync<List<RatingPosition>>(file).AsTask().Result!;
        }
        private static List<Quiz> DeserializeQuizes(FileStream file)
        {
            return JsonSerializer.DeserializeAsync<List<Quiz>>(file).AsTask().Result!;
        }
        private static List<User> DeserializeUsers(FileStream file)
        {
            return JsonSerializer.DeserializeAsync<List<User>>(file).AsTask().Result!;
        }
    }
}
