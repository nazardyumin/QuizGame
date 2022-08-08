public class DefaultFeatures
{
    private User _user;
    private UsersDataBase _database;
    public DefaultFeatures(User user)
    {
        _user = user;
        _database = new UsersDataBase();
    }
    public void ChangePassword(string NewPassword)
    {
        _database.LoadFromFile();
        var user = _database.SearchByLogin(_user.Login);
        user.Password = NewPassword;
        _database.SaveToFile();
        _database.Clear();
    }
    public void ChangeDateOfBirth(string NewDateOfBirth)
    {
        _database.LoadFromFile();
        var user = _database.SearchByLogin(_user.Login);
        user.DateOfBirth = NewDateOfBirth;
        _user.DateOfBirth = NewDateOfBirth;
        _database.SaveToFile();
        _database.Clear();
    }
}

