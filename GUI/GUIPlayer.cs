using QuizGame.Helpers;
using Terminal.Gui;
using NStack;

namespace QuizGame.GUI
{
    public class GUIPlayer : GUIDefault
    {
        private QuizPlayer _player;
        private int _iterator;
        private string _buffer_question;
        private List<string> _buffer_answers;
        private bool _is_playing;
        public GUIPlayer(User user) : base(user)
        {
            _player = new(user);
            _changePass = _player.ChangePassword;
            _changeDate = _player.ChangeDateOfBirth;
            _iterator = 0;
            _is_playing = false;
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
            var hello = new Label(GetPlayerInfo())
            {
                X = Pos.AnchorEnd(GetPlayerInfo().Length) - 1,
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
            var edit_quiz = new Button("Watch Results");
            edit_quiz.X = Pos.Center();
            edit_quiz.Y = Pos.Top(create_quiz) + 2;
            edit_quiz.Clicked += () =>
            {
                action = WatchResultsWindow;
                top.Running = false;
            };
            win.Add(create_quiz, edit_quiz);
            Application.Run();
            return (keep_on, logout, back, action);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) PlayQuizWindow()
        {
            bool logout;
            bool keep_on;
            bool back;
            SomeAction action = null;
            (bool keep_on, bool logout, bool back, SomeAction action) stop;
            do
            {
                stop = PlayQuizGamepLay();
                logout = stop.logout;
                keep_on = stop.keep_on;
                back = stop.back;
            } while (logout == false && keep_on == true && back == false);
            return (keep_on, logout, back, action);
        }

        private (bool keep_on, bool logout, bool back, SomeAction action) PlayQuizGamepLay()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction action = null; 
            Application.Init();
            int count;
            int zero_position = 0;
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
            {new MenuItem ("_Logout", "", () => { logout=true; top.Running = false; }),
             new MenuItem("_Quit", "", () => {  if (GUIHelper.Quit()) {keep_on = false; top.Running = false;} }) })});
            top.Add(menu);
            int quizes_passed = 0;
            int total_scores = 0;
            if(_user.Results is not null)
            {
                quizes_passed = _user.Results.Count();
                foreach (var item in _user.Results)
                {
                    total_scores += item.Scores;
                }
            }
            var hello = new Label(GetPlayerInfo())
            {
                X = Pos.AnchorEnd(GetPlayerInfo().Length) - 1,
                Y = Pos.AnchorEnd(1)
            };
            hello.ColorScheme = Colors.Menu;
            win.Add(hello);
            if (_is_playing == false)
            {
                var header = new Label("Available Quizes: ")
                {
                    X = Pos.Center(),
                    Y = 2
                };
                var buffer=new ustring[_player.GetAllQuizThemes().Count];
                for(int i=0;i<buffer.Length;i++)
                {
                    buffer[i] = _player.GetAllQuizThemes()[i];
                }
                var themes_list = new RadioGroup(buffer);
                themes_list.X = Pos.Center();
                themes_list.Y = Pos.Top(header) + 3;
                var play = new Button("Play");
                play.X = Pos.Center();
                play.Y = Pos.Bottom(themes_list) + 3;
                play.Clicked += () =>
                {
                    _is_playing = true;
                    _player.SetQuiz(_player.FindQuiz(_player.GetAllQuizThemes()[themes_list.SelectedItem]));
                    top.Running = false;
                };
                var mixed_quiz = new Button("MixedQuiz");
                mixed_quiz.X = Pos.Right(play)+2;
                mixed_quiz.Y = Pos.Top(play);
                mixed_quiz.Clicked += () =>
                {
                    _is_playing = true;
                    _player.SetQuiz(_player.MixedQuiz());
                    top.Running = false;
                };
                var cancel = new Button("Cancel");
                cancel.X = Pos.Left(play) - 12;
                cancel.Y = Pos.Top(play);
                cancel.Clicked += () =>
                {
                    back = true;
                    top.Running = false;
                };
                win.Add(header,themes_list,play,cancel,mixed_quiz);
            }
            else
            {
                count = _player.GetCount();
                _buffer_question = _player.GetQuestion(_iterator);
                _buffer_answers = _player.GetListAnswers(_iterator);
            }
            win.Add();
            Application.Run();
            return (keep_on, logout, back, action);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) WatchResultsWindow()
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
        private string GetPlayerInfo()
        {
            int quizes_passed = 0;
            int total_scores = 0;
            if (_user.Results is not null)
            {
                quizes_passed = _user.Results.Count();
                foreach (var item in _user.Results)
                {
                    total_scores += item.Scores;
                }
            }
            return $"Quizes passed: {quizes_passed} | Total Scores: {total_scores} | {_role}";
        }
    }
}