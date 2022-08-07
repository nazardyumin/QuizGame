namespace QuizGame.GUI
{
    public class GUIPlayer
    {
        private PlayerFeatures? _playerFeatures;
        public GUIPlayer(User user)
        {
            _playerFeatures = new PlayerFeatures(user);
        }

        public void Play()
        {
            _playerFeatures.Play();
        }










    }
}
