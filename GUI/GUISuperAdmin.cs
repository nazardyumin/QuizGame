namespace QuizGame.GUI
{
    public class GUISuperAdmin : GUIDefault
    {
        SuperAdminFeatures _features;
        public GUISuperAdmin(User user) : base(user)
        {
            _features = new(user);
            _changePass = _features.ChangePassword;
            _changeDate = _features.ChangeDateOfBirth;
        }
        public void CreateSMTH()
        {

        }
    }
}
