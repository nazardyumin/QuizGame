using System.Text.Json;
using QuizGame.Configs;
using QuizGame.Helpers;

namespace QuizGame.Users
{
    public class UsersDataBase
    {
        private List<User> users;
        private readonly string path;
        public UsersDataBase()
        {
            users = new List<User>();
            path = PathInit();
        }
        public void Add(User user)
        {
            users.Add(user);
        }
        public User? SearchByLogin(string login)
        {
            return users.Find((u) => u.Login == login);
        }
        public void SaveToFile()
        {
            File.Delete(path + "UsersList.json");
            using var file = new FileStream(path + "UsersList.json", FileMode.Create, FileAccess.Write);
            Serialize(file, users);
        }
        public void LoadFromFile()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                FileStream? helpfile = null;
                var file = new FileStream(path + "UsersList.json", FileMode.OpenOrCreate, FileAccess.Read);
                var newfile = SerializerHelper.IfEmptyUsersListFile(ref file, ref helpfile!, path + "UsersList.json");
                var list = Deserialize(newfile);
                newfile.Close();
                users = list!;
            }
            else
            {
                var file = new FileStream(path + "UsersList.json", FileMode.Open, FileAccess.Read);
                var list = Deserialize(file);
                file.Close();
                users = list!;
            }
        }
        public void Clear()
        {
            users.Clear();
        }
        private static string PathInit()
        {
            return PathsConfig.Init().PathToUserList!;
        }
        private List<User> Deserialize(FileStream file)
        {
            return JsonSerializer.DeserializeAsync<List<User>>(file).AsTask().Result!;
        }
        private static void Serialize(FileStream file, List<User> users)
        {
            JsonSerializer.SerializeAsync(file, users);
        }
    }
}
