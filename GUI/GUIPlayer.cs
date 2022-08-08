using QuizGame.Helpers;
using Terminal.Gui;

namespace QuizGame.GUI
{
    public class GUIPlayer : GUIDefault
    {
        PlayerFeatures _features;
        public GUIPlayer(User user) : base(user)
        {
            _features = new(user);
            _changePass = _features.ChangePassword;
            _changeDate = _features.ChangeDateOfBirth;
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) MainMenuWindow()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction action = null;
            Application.Init();
            var top = Application.Top;
            var win = new Window("QuizGame")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            top.Add(win);
            var menu = new MenuBar(new MenuBarItem[] {new MenuBarItem ("_Menu", new MenuItem []
            {new MenuItem ("_Settings", "", () =>{action=SettingsWindow; top.Running = false; }),
             new MenuItem ("_Logout", "", () => { logout=true; top.Running = false; }),
             new MenuItem("_Quit", "", () => {  if (GUIHelper.Quit()) {keep_on = false;top.Running = false; } }) })});
            top.Add(menu);
            var hello = new Label(_role)
            {
                X = Pos.AnchorEnd(_role.Length) - 1,
                Y = Pos.AnchorEnd(1)
            };
            hello.ColorScheme = Colors.Menu;
            win.Add(hello);
            var create_quiz = new Button("Play Quiz");
            create_quiz.X = Pos.Center();
            create_quiz.Y = 5;
            create_quiz.Clicked += () =>
            {
                action = PlayQuizWindow;
                top.Running = false;
            };
            var edit_quiz = new Button("Play Quiz");
            edit_quiz.X = Pos.Center();
            edit_quiz.Y = Pos.Top(create_quiz) + 2;
            edit_quiz.Clicked += () =>
            {
                action = PlayQuizWindow;
                top.Running = false;
            };
            win.Add(create_quiz, edit_quiz);
            Application.Run();
            return (keep_on, logout, back, action);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) PlayQuizWindow()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction action = null;
            return (keep_on, logout, back, action);
        }
        public (bool keep_on, bool logout) PlayerMenu()
        {
            bool keep_on = true;
            bool logout = false;
            do
            {
                var reply = MainMenuWindow();
                keep_on = reply.keep_on;
                logout = reply.logout;
                if (keep_on == false || logout == true) break;
                if (reply.action is not null)
                {
                    (bool keep_on, bool logout, bool back, SomeAction action) stop = reply.action.Invoke();
                    keep_on = stop.keep_on;
                    logout = stop.logout;
                }
            } while (logout == false && keep_on == true);
            return (keep_on, logout);
        }
    }
}
