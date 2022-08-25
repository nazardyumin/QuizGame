using QuizGame.Users;

namespace QuizGame.Features
{
    public class Default
    {
        protected readonly User _user;
        protected Default(User user)
        {
            _user = user;
        }
        public void ChangePassword(string newPassword)
        {
            var database = new UsersDataBase();
            database.LoadFromFile();
            var user = database.SearchByLogin(_user.Login!);
            user!.Password = newPassword;
            database.SaveToFile();
        }
        public void ChangeDateOfBirth(string newDateOfBirth)
        {
            var database = new UsersDataBase();
            database.LoadFromFile();
            var user = database.SearchByLogin(_user.Login!);
            user!.DateOfBirth = newDateOfBirth;
            _user.DateOfBirth = newDateOfBirth;
            database.SaveToFile();
        }
    }
}
