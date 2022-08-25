using System.Text.Json;
using QuizGame.Configs;
using QuizGame.Quizes;
using QuizGame.Quizes.QuizResults;
using QuizGame.Users;

namespace QuizGame.Helpers
{
    public static class SerializerHelper
    {
        public static FileStream IfEmptyRatingPositionFile(ref FileStream file, ref FileStream helpFile, string path)
        {
            if (file.Length == 0)
            {
                file.Close();
                var testRecord = new List<RatingPosition>();
                var tempFile = new FileStream(path, FileMode.Open, FileAccess.Write);
                JsonSerializer.SerializeAsync(tempFile, testRecord);
                tempFile.Close();
                helpFile = new FileStream(path, FileMode.Open, FileAccess.Read);
                return helpFile;
            }
            return file;
        }
        public static FileStream IfEmptyUsersListFile(ref FileStream file, ref FileStream helpFile, string path)
        {
            if (file.Length == 0)
            {
                file.Close();
                var newFile = new FileStream("DefaultUsersList.json", FileMode.Open, FileAccess.Read);
                var defaultUsers = DeserializeUsers(newFile);
                newFile.Close();
                var tempFile = new FileStream(path, FileMode.Open, FileAccess.Write);
                JsonSerializer.SerializeAsync(tempFile, defaultUsers);
                tempFile.Close();
                helpFile = new FileStream(path, FileMode.Open, FileAccess.Read);
                return helpFile;
            }
            return file;
        }
        public static FileStream IfEmptyQuizesListFile(ref FileStream file, ref FileStream helpFile, string path)
        {
            if (file.Length == 0)
            {
                file.Close();
                var newFile = new FileStream("DefaultQuizesList.json", FileMode.Open, FileAccess.Read);
                var defaultQuizes = DeserializeQuizes(newFile);
                newFile.Close();
                foreach (var item in defaultQuizes)
                {
                    Top20Serializer.CreateTop20File($"{item.Theme} ({item.Level})");
                }
                var tempFile = new FileStream(path, FileMode.Open, FileAccess.Write);
                JsonSerializer.SerializeAsync(tempFile, defaultQuizes);
                tempFile.Close();
                helpFile = new FileStream(path, FileMode.Open, FileAccess.Read);
                return helpFile;
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
                using var hsFile2 = new FileStream(path + "Highscores.json", FileMode.Create, FileAccess.Write);
                JsonSerializer.SerializeAsync(hsFile2, orderedHighScores);
            }
            else
            {
                highscores = new List<RatingPosition>
                {
                    position
                };
                File.Delete(path + "Highscores.json");
                using var hsFile3 = new FileStream(path + "Highscores.json", FileMode.Create, FileAccess.Write);
                JsonSerializer.SerializeAsync(hsFile3, highscores);
            }
        }
        public static List<RatingPosition>? LoadHighscores()
        {
            var path = PathsConfig.Init().PathToHighscores;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path!);
            }
            FileStream? helpFile = null;
            var hsFile1 = new FileStream(path + "Highscores.json", FileMode.OpenOrCreate, FileAccess.Read);
            var newFile = IfEmptyRatingPositionFile(ref hsFile1, ref helpFile!, path + "Highscores.json");
            var highscores = DeserializeRatings(newFile);
            newFile.Close();
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
