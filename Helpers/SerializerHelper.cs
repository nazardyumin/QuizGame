using System.Text.Json;

public class SerializerHelper
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
            var defaultusers = JsonSerializer.DeserializeAsync<List<User>>(newfile).Result;
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
            var defaultquizes = JsonSerializer.DeserializeAsync<List<Quiz>>(newfile).Result;
            newfile.Close();
            var tempfile = new FileStream(path, FileMode.Open, FileAccess.Write);
            JsonSerializer.SerializeAsync(tempfile, defaultquizes);
            tempfile.Close();
            helpfile = new FileStream(path, FileMode.Open, FileAccess.Read);
            return helpfile;
        }
        return file;
    }
    public static void SaveHighscores(string path, RatingPosition position)
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
            if (Highscores.Contains(Highscores.Find((i) => i.Name == position.Name)))
            {
                var item = Highscores.Find((i) => i.Name == position.Name);
                item.Scores += position.Scores;
            }
            else
            {
                Highscores.Add(position);
            }
            var orderedHighScores = Highscores.OrderByDescending((r) => r.Scores);
            using var hsfile2 = new FileStream(path + "Highscores.json", FileMode.Open, FileAccess.Write);
            hsfile2.Position = 0;
            JsonSerializer.SerializeAsync(hsfile2, orderedHighScores);
        }
        else
        {
            Highscores = new List<RatingPosition>();
            Highscores.Add(position);
            using var hsfile3 = new FileStream(path + "Highscores.json", FileMode.Open, FileAccess.Write);
            hsfile3.Position = 0;
            JsonSerializer.SerializeAsync(hsfile3, Highscores);
        }
    }
}


