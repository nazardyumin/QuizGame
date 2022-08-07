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
    public static FileStream IfEmptyQuizesFile(ref FileStream file, ref FileStream helpfile, string path)
    {
        if (file.Length == 0)
        {
            file.Close();
            var testrecord = new List<Quiz>();
            var tempfile = new FileStream(path, FileMode.Open, FileAccess.Write);
            JsonSerializer.SerializeAsync(tempfile, testrecord);
            tempfile.Close();
            helpfile = new FileStream(path, FileMode.Open, FileAccess.Read);
            return helpfile;
        }
        return file;
    }
}


