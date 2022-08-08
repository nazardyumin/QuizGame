using System.Text.Json;
public class UsersDataBase
{
    public List<User> Users;
    public string Path;
    public UsersDataBase()
    {
        Users = new List<User>();
        Path = PathInit();
    }
    public void Add(User user)
    {
        Users.Add(user);
    }
    public User? SearchByLogin(string login)
    {
        return Users.Find((u) => u.Login == login);
    }
    public void SaveToFile()
    {
        File.Delete(Path + "UsersList.json");
        using var file = new FileStream(Path + "UsersList.json", FileMode.Create, FileAccess.Write);
        JsonSerializer.SerializeAsync(file, Users);
    }
    public void LoadFromFile()
    {
        if (!Directory.Exists(Path))
        {
            Directory.CreateDirectory(Path);
        }
        FileStream? helpfile = null;
        var file = new FileStream(Path + "UsersList.json", FileMode.OpenOrCreate, FileAccess.Read);
        var newfile = SerializerHelper.IfEmptyUsersListFile(ref file, ref helpfile, Path + "UsersList.json");
        var list = JsonSerializer.DeserializeAsync<List<User>>(newfile).Result;
        newfile.Close();
        if (list is not null)
        {
            Users = list;
        }
    }
    public void Clear()
    {
        Users.Clear();
    }
    private string PathInit()
    {
        return PathsConfig.Init().PathToUserList;
    }
}