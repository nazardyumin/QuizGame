public class User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DateOfBirth { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsSuperAdmin { get; set; }
    //public User(string firstname, string lastname, string dateofbirth, string login, string password)
    //{
    //    FirstName = firstname;
    //    LastName = lastname;
    //    DateOfBirth = dateofbirth;
    //    Login = login;  
    //    Password = password;
    //    IsAdmin = false;
    //    IsSuperAdmin = false;    
    //}
    //public User(string firstname, string lastname, string dateofbirth, string login, string password, bool isadmin)
    //{
    //    FirstName = firstname;
    //    LastName = lastname;
    //    DateOfBirth = dateofbirth;
    //    Login = login;
    //    Password = password;
    //    IsAdmin = isadmin;
    //    IsSuperAdmin = false;
    //}
    //public User(string firstname, string lastname, string dateofbirth, string login, string password, bool isadmin, bool issuperadmin)
    //{
    //    FirstName = firstname;
    //    LastName = lastname;
    //    DateOfBirth = dateofbirth;
    //    Login = login;
    //    Password = password;
    //    IsAdmin = isadmin;
    //    IsSuperAdmin = issuperadmin;
    //}
    //List<int, string>? Results { get; set; } //изменить на нужный тип данных (очки,игры)
}

