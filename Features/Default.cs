public class Default
{
    protected User _user;
    protected Default(User user)
    {
        _user = user;
    }
    public void ChangePassword(string NewPassword)
    {
        var database = new UsersDataBase();
        database.LoadFromFile();
        var user = database.SearchByLogin(_user.Login);
        user.Password = NewPassword;
        database.SaveToFile();
    }
    public void ChangeDateOfBirth(string NewDateOfBirth)
    {
        var database = new UsersDataBase();
        database.LoadFromFile();
        var user = database.SearchByLogin(_user.Login);
        user.DateOfBirth = NewDateOfBirth;
        _user.DateOfBirth = NewDateOfBirth;
        database.SaveToFile();
    }
}