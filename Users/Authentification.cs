using QuizGame.Quizes.QuizResults;

namespace QuizGame.Users
{
    public class Authentification
    {
        private readonly UsersDataBase _usersDataBase;
        public Authentification()
        {
            _usersDataBase = new UsersDataBase();
            _usersDataBase.LoadFromFile();
        }
        public (string, bool, User? user) SignIn(string login, string password)
        {
            var user = _usersDataBase.SearchByLogin(login);
            if (user == null) return ("login", false, null);
            else
            {
                if (user.Password == password)
                {
                    return ("password", true, user);
                }
                else return ("password", false, null);
            }
        }
        public (bool, User? user) Register(string firstname, string lastname, string dateofbirth, string login, string password)
        {
            if (_usersDataBase.SearchByLogin(login) is not null)
            {
                return (false, null);
            }
            else
            {
                var user = new User
                {
                    FirstName = firstname,
                    LastName = lastname,
                    DateOfBirth = dateofbirth,
                    Login = login,
                    Password = password,
                    IsAdmin = false,
                    IsSuperAdmin = false
                };
                _usersDataBase.Add(user);
                SaveNewUser();
                QuizResultsSerializer.CreateQuizResultsUserFile(user.Login);
                return (true, user);
            }
        }
        public (bool, User? user) RegisterAdmin(string firstname, string lastname, string dateofbirth, string login, string password, int which_admin)
        {
            if (_usersDataBase.SearchByLogin(login) is not null)
            {
                return (false, null);
            }
            else
            {
                User user;
                if (which_admin == 0)
                {
                    user = new User
                    {
                        FirstName = firstname,
                        LastName = lastname,
                        DateOfBirth = dateofbirth,
                        Login = login,
                        Password = password,
                        IsAdmin = true,
                        IsSuperAdmin = false
                    };
                }
                else
                {
                    user = new User
                    {
                        FirstName = firstname,
                        LastName = lastname,
                        DateOfBirth = dateofbirth,
                        Login = login,
                        Password = password,
                        IsAdmin = false,
                        IsSuperAdmin = true
                    };
                }
                _usersDataBase.Add(user);
                SaveNewUser();
                return (true, user);
            }
        }
        private void SaveNewUser()
        {
            _usersDataBase.SaveToFile();
            _usersDataBase.Clear();
        }
    }
}
