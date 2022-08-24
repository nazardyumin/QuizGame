using NStack;
using QuizGame.Features;
using QuizGame.Helpers;
using QuizGame.Users;
using Terminal.Gui;

namespace QuizGame.GUI
{
    public class GuiPlayer : GuiDefault
    {
        private readonly QuizPlayer _player;
        private int _iterator;
        private string? _buffer_question { get; set; }
        private List<string> _buffer_answers { get; set; }
        private bool _is_playing;
        private bool _is_watching;
        private (bool answer1, bool answer2, bool answer3, bool answer4)[] _memory_bools;
        public GuiPlayer(User user) : base(user)
        {
            _player = new(user);
            _changePass = _player.ChangePassword;
            _changeDate = _player.ChangeDateOfBirth;
            _iterator = 0;
            _buffer_answers = new();
            _is_playing = false;
            _is_watching = false;
            _memory_bools = new (bool answer1, bool answer2, bool answer3, bool answer4)[1];
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) MainMenuWindow()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction? action = null;
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
             new MenuItem("_Quit", "", () => {  if (GuiHelper.Quit()) { keep_on = false;top.Running = false; } }) })});
            top.Add(menu);
            string buffer_role = $"{_player.GetPlayerInfo()}{_role}";
            var hello = new Label(buffer_role)
            {
                X = Pos.AnchorEnd(buffer_role.Length) - 1,
                Y = Pos.AnchorEnd(1),
                ColorScheme = Colors.Menu
            };
            win.Add(hello);
            var play_quiz = new Button("Play Quiz")
            {
                X = Pos.Center(),
                Y = 5
            };
            play_quiz.Clicked += () =>
            {
                action = PlayQuizWindow;
                top.Running = false;
            };
            var watch_results = new Button("Watch Results")
            {
                X = Pos.Center(),
                Y = Pos.Top(play_quiz) + 2
            };
            watch_results.Clicked += () =>
            {
                action = WatchResultsWindow;
                top.Running = false;
            };
            var watch_highscores = new Button("Watch Highscores")
            {
                X = Pos.Center(),
                Y = Pos.Top(watch_results) + 2
            };
            watch_highscores.Clicked += () =>
            {
                MessageBox.Query(30, 15, "Highscores", $"{_player.GetHighScores()}", "Ok");
            };
            win.Add(play_quiz, watch_results, watch_highscores);
            Application.Run();
            return (keep_on!, logout!, back!, action!);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) PlayQuizWindow()
        {
            bool logout;
            bool keep_on;
            bool back;
            SomeAction? action = null;
            (bool keep_on, bool logout, bool back, SomeAction action) stop;
            do
            {
                stop = PlayQuizGameplay();
                logout = stop.logout;
                keep_on = stop.keep_on;
                back = stop.back;
            } while (!logout && keep_on && !back);
            return (keep_on!, logout!, back!, action!);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) PlayQuizGameplay()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction? action = null;
            Application.Init();
            int count;
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
             new MenuItem("_Quit", "", () => {  if (GuiHelper.Quit()) { keep_on = false; top.Running = false;} }) })});
            top.Add(menu);
            string buffer_role = $"{_player.GetPlayerInfo()}{_role}";
            var hello = new Label(buffer_role)
            {
                X = Pos.AnchorEnd(buffer_role.Length) - 1,
                Y = Pos.AnchorEnd(1),
                ColorScheme = Colors.Menu
            };
            win.Add(hello);
            if (!_is_playing)
            {
                var header = new Label("Available Quizes: ")
                {
                    X = Pos.Center(),
                    Y = 2
                };
                if (_player.GetAllQuizThemes().Count == 0)
                {
                    var no_quizes = new Label("No Quizes available!")
                    {
                        X = Pos.Center(),
                        Y = Pos.Bottom(header) + 4
                    };
                    var return_back = new Button("Back")
                    {
                        X = Pos.Center(),
                        Y = Pos.Bottom(no_quizes) + 3
                    };
                    return_back.Clicked += () =>
                    {
                        back = true;
                        top.Running = false;
                    };
                    win.Add(no_quizes, return_back);
                }
                else
                {
                    var buffer = new ustring[_player.GetAllQuizThemes().Count];
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = _player.GetAllQuizThemes()[i];
                    }
                    var themes_list = new RadioGroup(buffer)
                    {
                        X = Pos.Center(),
                        Y = Pos.Top(header) + 3
                    };
                    var play = new Button("Play")
                    {
                        X = Pos.Center(),
                        Y = Pos.Bottom(themes_list) + 3
                    };
                    play.Clicked += () =>
                    {
                        _is_playing = true;
                        _player.SetQuiz(_player.FindQuiz(themes_list.SelectedItem)!);
                        top.Running = false;
                    };
                    var mixed_quiz = new Button("MixedQuiz")
                    {
                        X = Pos.Right(play) + 2,
                        Y = Pos.Top(play)
                    };
                    mixed_quiz.Clicked += () =>
                    {
                        _is_playing = true;
                        _player.SetQuiz(_player.MixedQuiz());
                        top.Running = false;
                    };
                    var cancel = new Button("Cancel")
                    {
                        X = Pos.Left(play) - 12,
                        Y = Pos.Top(play)
                    };
                    cancel.Clicked += () =>
                    {
                        back = true;
                        top.Running = false;
                    };
                    win.Add(header, themes_list, play, cancel, mixed_quiz);
                }
            }
            else
            {
                hello.Visible = false;
                menu.Visible = false;
                count = _player.GetCount();
                MemoryBoolsResize(ref _memory_bools, count);
                _buffer_question = _player.GetQuestion(_iterator);
                _buffer_answers = _player.GetListAnswers(_iterator);
                _player.ResetQuizResult();
                if (_player.GetTop20()!.Count > 0)
                {
                    if (_player.GetTop20()!.Count < 20)
                    {
                        var buffer = new ustring[_player.GetTop20()!.Count];
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            buffer[i] = _player.GetTop20()![i];
                        }
                        var items = new MenuItem[_player.GetTop20()!.Count];
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
                            buffer[i] = _player.GetTop20()![i];
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
                    items[0] = new MenuItem("No results available!", "", () => { });
                    var list_top20 = new MenuBarItem("_Top20", items);
                    var menu_top20 = new MenuBarItem[] { list_top20, };
                    var top20 = new MenuBar(menu_top20);
                    top.Add(top20);
                }
                var header = new Label($"{_player.GetTheme()} ({_player.GetLevel()})")
                {
                    X = Pos.Center(),
                    Y = 3
                };
                var question_label = new Label($"Question {_iterator + 1} of {count}")
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(header) + 6
                };
                var question = new Label(_buffer_question)
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(question_label) + 2,
                    ColorScheme = Colors.TopLevel
                };
                var answer1 = new CheckBox(_buffer_answers[0])
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(question) + 3
                };
                var answer2 = new CheckBox(_buffer_answers[1])
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(answer1) + 1
                };
                var answer3 = new CheckBox(_buffer_answers[2])
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(answer2) + 1
                };
                var answer4 = new CheckBox(_buffer_answers[3])
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(answer3) + 1
                };
                answer1.Checked = _memory_bools[_iterator].answer1;
                answer2.Checked = _memory_bools[_iterator].answer2;
                answer3.Checked = _memory_bools[_iterator].answer3;
                answer4.Checked = _memory_bools[_iterator].answer4;
                var previous_question = new Button("<")
                {
                    X = Pos.Center() - 6,
                    Y = Pos.Bottom(answer4) + 5
                };
                previous_question.Clicked += () =>
                {
                    _memory_bools[_iterator] = (answer1.Checked, answer2.Checked, answer3.Checked, answer4.Checked);
                    _iterator--;
                    top.Running = false;
                };
                var first_question = new Button("<<<")
                {
                    X = Pos.Left(previous_question) - 9,
                    Y = Pos.Bottom(answer4) + 5
                };
                first_question.Clicked += () =>
                {
                    _memory_bools[_iterator] = (answer1.Checked, answer2.Checked, answer3.Checked, answer4.Checked);
                    _iterator = 0;
                    top.Running = false;
                };
                var next_question = new Button(">")
                {
                    X = Pos.Right(previous_question) + 2,
                    Y = Pos.Bottom(answer4) + 5
                };
                next_question.Clicked += () =>
                {
                    _memory_bools[_iterator] = (answer1.Checked, answer2.Checked, answer3.Checked, answer4.Checked);
                    _iterator++;
                    top.Running = false;
                };
                var last_question = new Button(">>>")
                {
                    X = Pos.Right(next_question) + 2,
                    Y = Pos.Bottom(answer4) + 5
                };
                last_question.Clicked += () =>
                {
                    _memory_bools[_iterator] = (answer1.Checked, answer2.Checked, answer3.Checked, answer4.Checked);
                    _iterator = count - 1;
                    top.Running = false;
                };
                if (count <= 1)
                {
                    first_question.Visible = false;
                    previous_question.Visible = false;
                    next_question.Visible = false;
                    last_question.Visible = false;
                }
                else if (count == 2 && _iterator == 0)
                {
                    first_question.Visible = false;
                    previous_question.Visible = false;
                    next_question.Visible = true;
                    last_question.Visible = false;
                }
                else if (count == 2 && _iterator == 1)
                {
                    first_question.Visible = false;
                    previous_question.Visible = true;
                    next_question.Visible = false;
                    last_question.Visible = false;
                }
                else if (count >= 3 && _iterator == 0)
                {
                    first_question.Visible = false;
                    previous_question.Visible = false;
                    next_question.Visible = true;
                    last_question.Visible = true;
                }
                else if (count == 3 && _iterator == 1)
                {
                    first_question.Visible = false;
                    previous_question.Visible = true;
                    next_question.Visible = true;
                    last_question.Visible = false;
                }
                else if ((count == 3 && _iterator == 2) || (count > 3 && _iterator == count - 1))
                {
                    first_question.Visible = true;
                    previous_question.Visible = true;
                    next_question.Visible = false;
                    last_question.Visible = false;
                }
                else if (count > 3 && _iterator == 1)
                {
                    first_question.Visible = false;
                    previous_question.Visible = true;
                    next_question.Visible = true;
                    last_question.Visible = true;
                }
                else if (count > 3 && _iterator == count - 2)
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
                    next_question.Visible = true;
                    last_question.Visible = true;
                }
                var finish = new Button("Finish")
                {
                    X = Pos.Center() + 1,
                    Y = Pos.Bottom(previous_question) + 3
                };
                finish.Clicked += () =>
                {
                    _memory_bools[_iterator] = (answer1.Checked, answer2.Checked, answer3.Checked, answer4.Checked);
                    if (_iterator != count - 1)
                    {
                        if (GuiHelper.ForcedFinish())
                        {
                            for (int i = 0; i < count; i++)
                            {
                                _player.AddItemToQuizResult(i, _player.CheckingAnswer(i, _memory_bools[i].answer1, _memory_bools[i].answer2, _memory_bools[i].answer3, _memory_bools[i].answer4));
                            }
                            bool is_mixed = header.Text == "Mixed Quiz (Mixed)";
                            _player.SaveResults(is_mixed);
                            if (_player.GetScores() == 1)
                            {
                                MessageBox.Query(30, 7, "Quiz is passed!", "You got 1 point!", "Ok");
                            }
                            else
                            {
                                MessageBox.Query(30, 7, "Quiz is passed!", $"You got {_player.GetScores()} points!", "Ok");
                            }
                            _iterator = 0;
                            MemoryBoolsReset(count);
                            _is_playing = false;
                            top.Running = false;
                        }
                        else
                        {
                            top.Running = false;
                        }
                    }
                    else
                    {
                        if (GuiHelper.Finish())
                        {
                            for (int i = 0; i < count; i++)
                            {
                                _player.AddItemToQuizResult(i, _player.CheckingAnswer(i, _memory_bools[i].answer1, _memory_bools[i].answer2, _memory_bools[i].answer3, _memory_bools[i].answer4));
                            }
                            bool is_mixed = header.Text == "Mixed Quiz (Mixed)";
                            _player.SaveResults(is_mixed);
                            if (_player.GetScores() == 0)
                            {
                                MessageBox.ErrorQuery(30, 7, "Quiz is not passed!", "You got 0 points!", "Ok");
                            }
                            else if (_player.GetScores() == 1)
                            {
                                MessageBox.Query(30, 7, "Quiz is passed!", "You got 1 point!", "Ok");
                            }
                            else
                            {
                                MessageBox.Query(30, 7, "Quiz is passed!", $"You got {_player.GetScores()} points!", "Ok");
                            }
                            _iterator = 0;
                            MemoryBoolsReset(count);
                            _is_playing = false;
                            top.Running = false;
                        }
                        else
                        {
                            top.Running = false;
                        }
                    }
                };
                var cancel = new Button("Cancel")
                {
                    X = Pos.Left(finish) - 12,
                    Y = Pos.Top(finish)
                };
                cancel.Clicked += () =>
                {
                    _iterator = 0;
                    MemoryBoolsReset(count);
                    _is_playing = false;
                    top.Running = false;
                };
                win.Add(header, question_label, question, answer1, answer2, answer3, answer4, next_question, previous_question, first_question, last_question, finish, cancel);
            }
            Application.Run();
            return (keep_on!, logout!, back!, action!);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) WatchResultsWindow()
        {
            bool logout;
            bool keep_on;
            bool back;
            SomeAction? action = null;
            (bool keep_on, bool logout, bool back, SomeAction action) stop;
            do
            {
                stop = WatchResultsMenu();
                logout = stop.logout;
                keep_on = stop.keep_on;
                back = stop.back;
            } while (!logout && keep_on && !back);
            return (keep_on!, logout!, back!, action!);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) WatchResultsMenu()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction? action = null;
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
            {new MenuItem ("_Logout", "", () => { logout=true; top.Running = false; }),
             new MenuItem("_Quit", "", () => {  if (GuiHelper.Quit()) { keep_on = false; top.Running = false;} }) })});
            top.Add(menu);
            string buffer_role = $"{_player.GetPlayerInfo()}{_role}";
            var hello = new Label(buffer_role)
            {
                X = Pos.AnchorEnd(buffer_role.Length) - 1,
                Y = Pos.AnchorEnd(1),
                ColorScheme = Colors.Menu
            };
            win.Add(hello);
            var list = _player.GetQuizResults();
            if (!_is_watching)
            {
                var header = new Label("Available Results: ")
                {
                    X = Pos.Center(),
                    Y = 2
                };
                if (list!.Count > 0)
                {
                    int i = 0;
                    int space = 4;
                    var buttons = new Button[list.Count];
                    var iterators = new int[list.Count];
                    foreach (var item in list)
                    {
                        iterators[i] = i;
                        buttons[i] = new Button($"{item.Theme} ({item.Level}) {item.Date}")
                        {
                            X = Pos.Center(),
                            Y = Pos.Bottom(header) + space
                        };
                        i++;
                        space += 2;
                    }
                    int j = 0;
                    foreach (var item in iterators)
                    {
                        buttons[j].Clicked += () =>
                        {
                            _iterator = item;
                            _is_watching = true;
                            top.Running = false;
                        };
                        j++;
                    }
                    win.Add(buttons);
                    var return_back = new Button("Back")
                    {
                        X = Pos.Center(),
                        Y = Pos.Bottom(buttons[^1]) + 3
                    };
                    return_back.Clicked += () =>
                    {
                        back = true;
                        top.Running = false;
                    };
                    win.Add(header, return_back);
                }
                else
                {
                    var no_results = new Label("No results available!")
                    {
                        X = Pos.Center(),
                        Y = Pos.Bottom(header) + 4
                    };
                    var return_back = new Button("Back")
                    {
                        X = Pos.Center(),
                        Y = Pos.Bottom(no_results) + 3
                    };
                    return_back.Clicked += () =>
                    {
                        back = true;
                        top.Running = false;
                    };
                    win.Add(no_results, return_back);
                }
            }
            else
            {
                var theme = new Label($"{list![_iterator].Theme} ({list[_iterator].Level}) {list[_iterator].Date}")
                {
                    X = Pos.Center(),
                    Y = 2
                };
                var scores = new Label($"Scores: {list[_iterator].Scores} of {list![_iterator].AnsweredQuestions!.Count}")
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(theme) + 2,
                };
                var questions = list[_iterator].AnsweredQuestions;
                var question_labels = new Label[questions!.Count];
                int i = 0;
                int space = 4;
                foreach (var item in questions)
                {
                    question_labels[i] = new Label($"{item.Question}")
                    {
                        X = Pos.Center(),
                        Y = Pos.Bottom(scores) + space
                    };
                    if (item.IsCorrect) question_labels[i].ColorScheme = Colors.TopLevel;
                    else question_labels[i].ColorScheme = Colors.Error;
                    i++;
                    space += 2;
                }
                var return_back = new Button("Back")
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(question_labels[^1]) + 3,
                };
                return_back.Clicked += () =>
                {
                    _iterator = 0;
                    _is_watching = false;
                    top.Running = false;
                };
                win.Add(question_labels);
                win.Add(theme, scores, return_back);
            }
            Application.Run();
            return (keep_on!, logout!, back!, action!);
        }
        public (bool keep_on, bool logout) PlayerMenu()
        {
            bool keep_on;
            bool logout;
            do
            {
                var reply = MainMenuWindow();
                keep_on = reply.keep_on;
                logout = reply.logout;
                if (!keep_on || logout) break;
                if (reply.action is not null)
                {
                    (bool keep_on, bool logout, bool back, SomeAction action) stop = reply.action.Invoke();
                    keep_on = stop.keep_on;
                    logout = stop.logout;
                }
            } while (!logout && keep_on);
            return (keep_on, logout);
        }
        private static void MemoryBoolsResize(ref (bool answer1, bool answer2, bool answer3, bool answer4)[] array, int count)
        {
            if (array.Length != count)
            {
                Array.Resize(ref array, count);
            }
        }
        private void MemoryBoolsReset(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _memory_bools[i] = (false, false, false, false);
            }
        }
    }
}
