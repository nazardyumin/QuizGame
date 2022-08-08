namespace QuizGame.GUI
{
    public class GUIPlayer : GUIDefault
    {
        PlayerFeatures _features;
        public GUIPlayer(User user):base(user)
        {
            _features = new(user);
            _changePass = _features.ChangePassword;
            _changeDate = _features.ChangeDateOfBirth;
        }
        public void Play()
        {

        }
    }
}
