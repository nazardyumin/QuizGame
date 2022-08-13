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
        private (bool answer1, bool answer2, bool answer3, bool answer4)[] _memory_bools;
        public GUIPlayer(User user) : base(user)
        {
            _player = new(user);
            _changePass = _player.ChangePassword;
            _changeDate = _player.ChangeDateOfBirth;
            _iterator = 0;
            _buffer_answers = new();
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
             new MenuItem ("_Logout", "", () => {  logout=true; top.Running = false; }),
             new MenuItem("_Quit", "", () => {  if (GUIHelper.Quit()) { keep_on = false;top.Running = false; } }) })});
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
             new MenuItem("_Quit", "", () => {  if (GUIHelper.Quit()) { keep_on = false; top.Running = false;} }) })});
            top.Add(menu);
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
                    _player.SetQuiz(_player.FindQuiz(themes_list.SelectedItem));
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
                hello.Visible = false;
                menu.Visible = false;
                count = _player.GetCount();
                MemoryBoolsInit(count);
                MemoryBoolsResize(ref _memory_bools, count);
                _buffer_question = _player.GetQuestion(_iterator);
                _buffer_answers = _player.GetListAnswers(_iterator);
                _player.ResetQuizResult();
                if (_player.GetTop20().Count()>0)
                {
                    if (_player.GetTop20().Count()<20)
                    {
                        var buffer = new ustring[_player.GetTop20().Count()];
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            buffer[i] = _player.GetTop20()[i];
                        }
                        var items = new MenuItem[_player.GetTop20().Count()];
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            items[i] = new MenuItem(buffer[i], "", () => { });
                        }
                        var list_top20 = new MenuBarItem("_Top20", items);
                        var menu_top20 = new MenuBarItem[] { list_top20, };
                        var top20 = new MenuBar(menu_top20);
                        top.Add(top20);
                    }
                    else
                    {
                        var buffer = new ustring[20];
                        for (int i = 0; i < 20; i++)
                        {
                            buffer[i] = _player.GetTop20()[i];
                        }
                        var items = new MenuItem[20];
                        for (int i = 0; i < 20; i++)
                        {
                            items[i] = new MenuItem(buffer[i], "", () => { });
                        }
                        var list_top20 = new MenuBarItem("_Top20", items);
                        var menu_top20 = new MenuBarItem[] { list_top20 };
                        var top20 = new MenuBar(menu_top20);
                        top.Add(top20);
                    }                   
                }
                else
                {
                    var items = new MenuItem[2];
                    items[0] = new MenuItem("No results available!","" ,() => { });
                    var list_top20 = new MenuBarItem("_Top20", items);
                    var menu_top20 = new MenuBarItem[] { list_top20, };
                    var top20 = new MenuBar(menu_top20);
                    top.Add(top20);
                }
                var header = new Label($"{_player.GetTheme()} ({_player.GetLevel()})");
                header.X = Pos.Center();
                header.Y = 3;
                var question_label = new Label($"Question {_iterator+1} of {count}");
                question_label.X = Pos.Center();
                question_label.Y = Pos.Bottom(header)+6;
                var question=new Label(_buffer_question);
                question.X = Pos.Center();
                question.Y = Pos.Bottom(question_label) + 2;
                question.ColorScheme = Colors.TopLevel;
                var answer1=new CheckBox(_buffer_answers[0]);
                answer1.X= Pos.Center();
                answer1.Y= Pos.Bottom(question)+3;;
                var answer2 = new CheckBox(_buffer_answers[1]);
                answer2.X = Pos.Center();
                answer2.Y = Pos.Bottom(answer1) + 1;
                var answer3 = new CheckBox(_buffer_answers[2]);
                answer3.X = Pos.Center();
                answer3.Y = Pos.Bottom(answer2) + 1;
                var answer4 = new CheckBox(_buffer_answers[3]);
                answer4.X = Pos.Center();
                answer4.Y = Pos.Bottom(answer3) + 1;
                var previous_question = new Button("<");
                previous_question.X = Pos.Center() - 6;
                previous_question.Y = Pos.Bottom(answer4) + 5;
                previous_question.Clicked += () =>
                {
                    _memory_bools[_iterator] = (answer1.Checked, answer2.Checked, answer3.Checked, answer4.Checked);
                    _iterator--;
                    top.Running = false;
                };
                var first_question = new Button("<<<");
                first_question.X = Pos.Left(previous_question) - 9;
                first_question.Y = Pos.Bottom(answer4) + 5;
                first_question.Clicked += () =>
                {
                    _memory_bools[_iterator] = (answer1.Checked, answer2.Checked, answer3.Checked, answer4.Checked);
                    _iterator =0;
                    top.Running = false;
                };
                var next_question = new Button(">");
                next_question.X = Pos.Right(previous_question)+2;
                next_question.Y = Pos.Bottom(answer4) + 5;
                next_question.Clicked += () =>
                {
                    _memory_bools[_iterator] = (answer1.Checked, answer2.Checked, answer3.Checked, answer4.Checked);
                    _iterator++;
                    top.Running = false;
                };
                var last_question = new Button(">>>");
                last_question.X = Pos.Right(next_question) + 2;
                last_question.Y = Pos.Bottom(answer4) + 5;
                last_question.Clicked += () =>
                {
                    _memory_bools[_iterator] = (answer1.Checked, answer2.Checked, answer3.Checked, answer4.Checked);
                    _iterator = count-1;
                    top.Running = false;
                };
                if (count > 3)
                {
                    if (_iterator == 0)
                    {
                        first_question.Visible = false;
                        previous_question.Visible = false;
                        next_question.Visible = true;
                        last_question.Visible = true;
                    }
                    else if (_iterator == 1)
                    {
                        first_question.Visible = false;
                        previous_question.Visible = true;
                        next_question.Visible = true;
                        last_question.Visible = true;
                    }
                    else if (_iterator > 1 && _iterator < count - 1)
                    {
                        first_question.Visible = true;
                        previous_question.Visible = true;
                        next_question.Visible = true;
                        if (_iterator < count - 2) last_question.Visible = true;
                        else last_question.Visible = false;
                    }
                    else
                    {
                        first_question.Visible = true;
                        previous_question.Visible = true;
                        next_question.Visible = false;
                        last_question.Visible = false;
                    }
                }
                else
                {
                    if (_iterator == 0)
                    {
                        first_question.Visible = false;
                        previous_question.Visible = false;
                        next_question.Visible = true;
                        last_question.Visible = true;
                    }
                    else if (_iterator == 1)
                    {
                        first_question.Visible = false;
                        previous_question.Visible = true;
                        next_question.Visible = true;
                        last_question.Visible = false;
                    }
                    else if (_iterator > 1 && _iterator < count - 1)
                    {
                        first_question.Visible = true;
                        previous_question.Visible = true;
                        next_question.Visible = true;
                        last_question.Visible = false;
                    }
                    else
                    {
                        first_question.Visible = true;
                        previous_question.Visible = true;
                        next_question.Visible = false;
                        last_question.Visible = false;
                    }
                }
                var finish = new Button("Finish");
                finish.X = Pos.Center()+1;
                finish.Y = Pos.Bottom(previous_question) + 3;
                finish.Clicked += () =>
                {
                    _memory_bools[_iterator] = (answer1.Checked, answer2.Checked, answer3.Checked, answer4.Checked);
                    if (_iterator != count - 1)
                    {
                        if (GUIHelper.ForcedFinish())
                        {
                            var list_answers = new List<QuizQuestionResult>();
                            for (int i = 0; i < count; i++)
                            {
                                _player.AddItemToQuizResult(i, _player.CheckingAnswer(i, _memory_bools[i].answer1, _memory_bools[i].answer2, _memory_bools[i].answer3, _memory_bools[i].answer4));
                            }
                            bool is_mixed = header.Text == "Mixed Quiz (Mixed)";
                            _player.SaveResults(is_mixed);
                            MessageBox.Query(30, 7, "Quiz is passed!", $"You got {_player.GetScores()} points!", "Ok");
                            _iterator = 0;
                            _is_playing = false;
                            top.Running = false;
                        }
                    }
                    else
                    {
                        if (GUIHelper.Finish())
                        {                        
                            var list_answers = new List<QuizQuestionResult>();
                            for (int i = 0; i < count; i++)
                            {
                                _player.AddItemToQuizResult(i, _player.CheckingAnswer(i, _memory_bools[i].answer1, _memory_bools[i].answer2, _memory_bools[i].answer3, _memory_bools[i].answer4));
                            }
                            bool is_mixed = header.Text == "Mixed Quiz (Mixed)";
                            _player.SaveResults(is_mixed);
                            MessageBox.Query(30, 7, "Quiz is passed!", $"You got {_player.GetScores()} points!", "Ok");
                            _iterator = 0;
                            _is_playing = false;
                            top.Running = false;
                        }
                    }         
                };
                var cancel = new Button("Cancel");
                cancel.X = Pos.Left(finish) -12;
                cancel.Y = Pos.Top(finish);
                cancel.Clicked += () =>
                {
                    _iterator = 0;
                    _is_playing = false;
                    back = true;
                    top.Running = false;
                };

                win.Add(header, question_label, question, answer1, answer2, answer3, answer4, next_question, previous_question, first_question, last_question, finish, cancel);
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
            var database = new UsersDataBase();
            database.LoadFromFile();
            int index = database.Users.IndexOf(database.SearchByLogin(_user.Login));
            if (database.Users[index].Results is not null)
            {
                quizes_passed = database.Users[index].Results.Count();
                foreach (var item in database.Users[index].Results)
                {
                    total_scores += item.Scores;
                }
            }
            return $"Quizes passed: {quizes_passed} | Total Scores: {total_scores} | {_role}";
        }
        private void MemoryBoolsInit(int count)
        {
            if (_memory_bools is null)
            {
                _memory_bools = new (bool answer1, bool answer2, bool answer3, bool answer4)[count];
            }
        }
        private void MemoryBoolsResize(ref (bool answer1, bool answer2, bool answer3, bool answer4) [] array , int count)
        {
            Array.Resize<(bool answer1, bool answer2, bool answer3, bool answer4)>(ref array,count);
        }
    }
}