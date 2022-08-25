using System.Text.Json;
using QuizGame.Configs;
using QuizGame.Helpers;

namespace QuizGame.Users
{
    public class UsersDataBase
    {
        private List<User> _users;
        private readonly string _path;
        public UsersDataBase()
        {
            _users = new List<User>();
            _path = PathInit();
        }
        public void Add(User user)
        {
            _users.Add(user);
        }
        public User? SearchByLogin(string login)
        {
            return _users.Find((u) => u.Login == login);
        }
        public void SaveToFile()
        {
            File.Delete(_path + "UsersList.json");
            using var file = new FileStream(_path + "UsersList.json", FileMode.Create, FileAccess.Write);
            Serialize(file, _users);
        }
        public void LoadFromFile()
        {
            if (!Directory.Exists(_path)||!File.Exists(_path + "UsersList.json"))
            {
                if(!Directory.Exists(_path))
                {
                    Directory.CreateDirectory(_path);
                }
                FileStream? helpFile = null;
                var file = new FileStream(_path + "UsersList.json", FileMode.OpenOrCreate, FileAccess.Read);
                var newFile = SerializerHelper.IfEmptyUsersListFile(ref file, ref helpFile!, _path + "UsersList.json");
                var list = Deserialize(newFile);
                newFile.Close();
                _users = list;
            }
            else
            {
                var file = new FileStream(_path + "UsersList.json", FileMode.Open, FileAccess.Read);
                var list = Deserialize(file);
                file.Close();
                _users = list;
            }
        }
        public void Clear()
        {
            _users.Clear();
        }
        private string PathInit()
        {
            return PathsConfig.Init().PathToUserList!;
        }
        private List<User> Deserialize(FileStream file)
        {
            return JsonSerializer.Deserialize<List<User>>(file)!;
        }
        private void Serialize(FileStream file, List<User> users)
        {
            JsonSerializer.Serialize(file, users);
        }
    }
}
