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
        private string? BufferQuestion { get; set; }
        private List<string> BufferAnswers { get; set; }
        private bool _isPlaying;
        private bool _isWatching;
        private (bool answer1, bool answer2, bool answer3, bool answer4)[] _memoryBools;
        public GuiPlayer(User user) : base(user)
        {
            _player = new(user);
            _changePass = _player.ChangePassword;
            _changeDate = _player.ChangeDateOfBirth;
            _iterator = 0;
            BufferAnswers = new();
            _isPlaying = false;
            _isWatching = false;
            _memoryBools = new (bool answer1, bool answer2, bool answer3, bool answer4)[1];
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) MainMenuWindow()
        {
            bool logout = false;
            bool keepOn = true;
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
             new MenuItem("_Quit", "", () => {  if (GuiHelper.Quit()) { keepOn = false;top.Running = false; } }) })});
            top.Add(menu);
            string bufferRole = $"{_player.GetPlayerInfo()}{_role}";
            var hello = new Label(bufferRole)
            {
                X = Pos.AnchorEnd(bufferRole.Length) - 1,
                Y = Pos.AnchorEnd(1),
                ColorScheme = Colors.Menu
            };
            win.Add(hello);
            var playQuiz = new Button("Play Quiz")
            {
                X = Pos.Center(),
                Y = 5
            };
            playQuiz.Clicked += () =>
            {
                action = PlayQuizWindow;
                top.Running = false;
            };
            var watchResults = new Button("Watch Results")
            {
                X = Pos.Center(),
                Y = Pos.Top(playQuiz) + 2
            };
            watchResults.Clicked += () =>
            {
                action = WatchResultsWindow;
                top.Running = false;
            };
            var watchHighscores = new Button("Watch Highscores")
            {
                X = Pos.Center(),
                Y = Pos.Top(watchResults) + 2
            };
            watchHighscores.Clicked += () =>
            {
                MessageBox.Query(30, 15, "Highscores", $"{_player.GetHighScores()}", "Ok");
            };
            win.Add(playQuiz, watchResults, watchHighscores);
            Application.Run();
            return (keepOn, logout, back, action!);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) PlayQuizWindow()
        {
            bool logout;
            bool keepOn;
            bool back;
            SomeAction? action = null;
            do
            {
                var stop = PlayQuizGameplay();
                logout = stop.logout;
                keepOn = stop.keep_on;
                back = stop.back;
            } while (!logout && keepOn && !back);
            return (keepOn, logout, back, action!);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) PlayQuizGameplay()
        {
            bool logout = false;
            bool keepOn = true;
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
             new MenuItem("_Quit", "", () => {  if (GuiHelper.Quit()) { keepOn = false; top.Running = false;} }) })});
            top.Add(menu);
            string bufferRole = $"{_player.GetPlayerInfo()}{_role}";
            var hello = new Label(bufferRole)
            {
                X = Pos.AnchorEnd(bufferRole.Length) - 1,
                Y = Pos.AnchorEnd(1),
                ColorScheme = Colors.Menu
            };
            win.Add(hello);
            if (!_isPlaying)
            {
                var header = new Label("Available Quizes: ")
                {
                    X = Pos.Center(),
                    Y = 2
                };
                if (_player.GetAllQuizThemes().Count == 0)
                {
                    var noQuizes = new Label("No Quizes available!")
                    {
                        X = Pos.Center(),
                        Y = Pos.Bottom(header) + 4
                    };
                    var returnBack = new Button("Back")
                    {
                        X = Pos.Center(),
                        Y = Pos.Bottom(noQuizes) + 3
                    };
                    returnBack.Clicked += () =>
                    {
                        back = true;
                        top.Running = false;
                    };
                    win.Add(noQuizes, returnBack);
                }
                else
                {
                    var buffer = new ustring[_player.GetAllQuizThemes().Count];
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = _player.GetAllQuizThemes()[i];
                    }
                    var themesList = new RadioGroup(buffer)
                    {
                        X = Pos.Center(),
                        Y = Pos.Top(header) + 3
                    };
                    var play = new Button("Play")
                    {
                        X = Pos.Center(),
                        Y = Pos.Bottom(themesList) + 3
                    };
                    play.Clicked += () =>
                    {
                        _isPlaying = true;
                        _player.SetQuiz(_player.FindQuiz(themesList.SelectedItem));
                        top.Running = false;
                    };
                    var mixedQuiz = new Button("MixedQuiz")
                    {
                        X = Pos.Right(play) + 2,
                        Y = Pos.Top(play)
                    };
                    mixedQuiz.Clicked += () =>
                    {
                        _isPlaying = true;
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
                    win.Add(header, themesList, play, cancel, mixedQuiz);
                }
            }
            else
            {
                hello.Visible = false;
                menu.Visible = false;
                count = _player.GetCount();
                MemoryBoolsResize(ref _memoryBools, count);
                BufferQuestion = _player.GetQuestion(_iterator);
                BufferAnswers = _player.GetListAnswers(_iterator);
                _player.ResetQuizResult();
                if (_player.GetTop20().Count > 0)
                {
                    if (_player.GetTop20().Count < 20)
                    {
                        var buffer = new ustring[_player.GetTop20().Count];
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            buffer[i] = _player.GetTop20()[i];
                        }
                        var items = new MenuItem[_player.GetTop20().Count];
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            items[i] = new MenuItem(buffer[i], "", () => { });
                        }
                        var listTop20 = new MenuBarItem("_Top20", items);
                        var menuTop20 = new MenuBarItem[] { listTop20, };
                        var top20 = new MenuBar(menuTop20);
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
                        var listTop20 = new MenuBarItem("_Top20", items);
                        var menuTop20 = new MenuBarItem[] { listTop20 };
                        var top20 = new MenuBar(menuTop20);
                        top.Add(top20);
                    }
                }
                else
                {
                    var items = new MenuItem[2];
                    items[0] = new MenuItem("No results available!", "", () => { });
                    var listTop20 = new MenuBarItem("_Top20", items);
                    var menuTop20 = new MenuBarItem[] { listTop20, };
                    var top20 = new MenuBar(menuTop20);
                    top.Add(top20);
                }
                var header = new Label($"{_player.GetTheme()} ({_player.GetLevel()})")
                {
                    X = Pos.Center(),
                    Y = 3
                };
                var questionLabel = new Label($"Question {_iterator + 1} of {count}")
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(header) + 6
                };
                var question = new Label(BufferQuestion)
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(questionLabel) + 2,
                    ColorScheme = Colors.TopLevel
                };
                var answer1 = new CheckBox(BufferAnswers[0])
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(question) + 3
                };
                var answer2 = new CheckBox(BufferAnswers[1])
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(answer1) + 1
                };
                var answer3 = new CheckBox(BufferAnswers[2])
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(answer2) + 1
                };
                var answer4 = new CheckBox(BufferAnswers[3])
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(answer3) + 1
                };
                answer1.Checked = _memoryBools[_iterator].answer1;
                answer2.Checked = _memoryBools[_iterator].answer2;
                answer3.Checked = _memoryBools[_iterator].answer3;
                answer4.Checked = _memoryBools[_iterator].answer4;
                var previousQuestion = new Button("<")
                {
                    X = Pos.Center() - 6,
                    Y = Pos.Bottom(answer4) + 5
                };
                previousQuestion.Clicked += () =>
                {
                    _memoryBools[_iterator] = (answer1.Checked, answer2.Checked, answer3.Checked, answer4.Checked);
                    _iterator--;
                    top.Running = false;
                };
                var firstQuestion = new Button("<<<")
                {
                    X = Pos.Left(previousQuestion) - 9,
                    Y = Pos.Bottom(answer4) + 5
                };
                firstQuestion.Clicked += () =>
                {
                    _memoryBools[_iterator] = (answer1.Checked, answer2.Checked, answer3.Checked, answer4.Checked);
                    _iterator = 0;
                    top.Running = false;
                };
                var nextQuestion = new Button(">")
                {
                    X = Pos.Right(previousQuestion) + 2,
                    Y = Pos.Bottom(answer4) + 5
                };
                nextQuestion.Clicked += () =>
                {
                    _memoryBools[_iterator] = (answer1.Checked, answer2.Checked, answer3.Checked, answer4.Checked);
                    _iterator++;
                    top.Running = false;
                };
                var lastQuestion = new Button(">>>")
                {
                    X = Pos.Right(nextQuestion) + 2,
                    Y = Pos.Bottom(answer4) + 5
                };
                lastQuestion.Clicked += () =>
                {
                    _memoryBools[_iterator] = (answer1.Checked, answer2.Checked, answer3.Checked, answer4.Checked);
                    _iterator = count - 1;
                    top.Running = false;
                };
                if (count <= 1)
                {
                    firstQuestion.Visible = false;
                    previousQuestion.Visible = false;
                    nextQuestion.Visible = false;
                    lastQuestion.Visible = false;
                }
                else if (count == 2 && _iterator == 0)
                {
                    firstQuestion.Visible = false;
                    previousQuestion.Visible = false;
                    nextQuestion.Visible = true;
                    lastQuestion.Visible = false;
                }
                else if (count == 2 && _iterator == 1)
                {
                    firstQuestion.Visible = false;
                    previousQuestion.Visible = true;
                    nextQuestion.Visible = false;
                    lastQuestion.Visible = false;
                }
                else if (count >= 3 && _iterator == 0)
                {
                    firstQuestion.Visible = false;
                    previousQuestion.Visible = false;
                    nextQuestion.Visible = true;
                    lastQuestion.Visible = true;
                }
                else if (count == 3 && _iterator == 1)
                {
                    firstQuestion.Visible = false;
                    previousQuestion.Visible = true;
                    nextQuestion.Visible = true;
                    lastQuestion.Visible = false;
                }
                else if ((count == 3 && _iterator == 2) || (count > 3 && _iterator == count - 1))
                {
                    firstQuestion.Visible = true;
                    previousQuestion.Visible = true;
                    nextQuestion.Visible = false;
                    lastQuestion.Visible = false;
                }
                else if (count > 3 && _iterator == 1)
                {
                    firstQuestion.Visible = false;
                    previousQuestion.Visible = true;
                    nextQuestion.Visible = true;
                    lastQuestion.Visible = true;
                }
                else if (count > 3 && _iterator == count - 2)
                {
                    firstQuestion.Visible = true;
                    previousQuestion.Visible = true;
                    nextQuestion.Visible = true;
                    lastQuestion.Visible = false;
                }
                else
                {
                    firstQuestion.Visible = true;
                    previousQuestion.Visible = true;
                    nextQuestion.Visible = true;
                    lastQuestion.Visible = true;
                }
                var finish = new Button("Finish")
                {
                    X = Pos.Center() + 1,
                    Y = Pos.Bottom(previousQuestion) + 3
                };
                finish.Clicked += () =>
                {
                    _memoryBools[_iterator] = (answer1.Checked, answer2.Checked, answer3.Checked, answer4.Checked);
                    if (_iterator != count - 1)
                    {
                        if (GuiHelper.ForcedFinish())
                        {
                            for (int i = 0; i < count; i++)
                            {
                                _player.AddItemToQuizResult(i, _player.CheckingAnswer(i, _memoryBools[i].answer1, _memoryBools[i].answer2, _memoryBools[i].answer3, _memoryBools[i].answer4));
                            }
                            bool isMixed = header.Text == "Mixed Quiz (Mixed)";
                            _player.SaveResults(isMixed);
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
                            _isPlaying = false;
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
                                _player.AddItemToQuizResult(i, _player.CheckingAnswer(i, _memoryBools[i].answer1, _memoryBools[i].answer2, _memoryBools[i].answer3, _memoryBools[i].answer4));
                            }
                            bool isMixed = header.Text == "Mixed Quiz (Mixed)";
                            _player.SaveResults(isMixed);
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
                            _isPlaying = false;
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
                    _isPlaying = false;
                    top.Running = false;
                };
                win.Add(header, questionLabel, question, answer1, answer2, answer3, answer4, nextQuestion, previousQuestion, firstQuestion, lastQuestion, finish, cancel);
            }
            Application.Run();
            return (keepOn, logout, back, action!);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) WatchResultsWindow()
        {
            bool logout;
            bool keepOn;
            bool back;
            SomeAction? action = null;
            do
            {
                var stop = WatchResultsMenu();
                logout = stop.logout;
                keepOn = stop.keep_on;
                back = stop.back;
            } while (!logout && keepOn && !back);
            return (keepOn, logout, back, action!);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) WatchResultsMenu()
        {
            bool logout = false;
            bool keepOn = true;
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
             new MenuItem("_Quit", "", () => {  if (GuiHelper.Quit()) { keepOn = false; top.Running = false;} }) })});
            top.Add(menu);
            string bufferRole = $"{_player.GetPlayerInfo()}{_role}";
            var hello = new Label(bufferRole)
            {
                X = Pos.AnchorEnd(bufferRole.Length) - 1,
                Y = Pos.AnchorEnd(1),
                ColorScheme = Colors.Menu
            };
            win.Add(hello);
            var list = _player.GetQuizResults();
            if (!_isWatching)
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
                            _isWatching = true;
                            top.Running = false;
                        };
                        j++;
                    }
                    win.Add(buttons);
                    var returnBack = new Button("Back")
                    {
                        X = Pos.Center(),
                        Y = Pos.Bottom(buttons[^1]) + 3
                    };
                    returnBack.Clicked += () =>
                    {
                        back = true;
                        top.Running = false;
                    };
                    win.Add(header, returnBack);
                }
                else
                {
                    var noResults = new Label("No results available!")
                    {
                        X = Pos.Center(),
                        Y = Pos.Bottom(header) + 4
                    };
                    var returnBack = new Button("Back")
                    {
                        X = Pos.Center(),
                        Y = Pos.Bottom(noResults) + 3
                    };
                    returnBack.Clicked += () =>
                    {
                        back = true;
                        top.Running = false;
                    };
                    win.Add(noResults, returnBack);
                }
            }
            else
            {
                var theme = new Label($"{list![_iterator].Theme} ({list[_iterator].Level}) {list[_iterator].Date}")
                {
                    X = Pos.Center(),
                    Y = 2
                };
                var scores = new Label($"Scores: {list[_iterator].Scores} of {list[_iterator].AnsweredQuestions!.Count}")
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(theme) + 2,
                };
                var questions = list[_iterator].AnsweredQuestions;
                var questionLabels = new Label[questions!.Count];
                int i = 0;
                int space = 4;
                foreach (var item in questions)
                {
                    questionLabels[i] = new Label($"{item.Question}")
                    {
                        X = Pos.Center(),
                        Y = Pos.Bottom(scores) + space
                    };
                    questionLabels[i].ColorScheme = item.IsCorrect ? Colors.TopLevel : Colors.Error;
                    i++;
                    space += 2;
                }
                var returnBack = new Button("Back")
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(questionLabels[^1]) + 3,
                };
                returnBack.Clicked += () =>
                {
                    _iterator = 0;
                    _isWatching = false;
                    top.Running = false;
                };
                win.Add(questionLabels);
                win.Add(theme, scores, returnBack);
            }
            Application.Run();
            return (keepOn, logout, back, action!);
        }
        public (bool keep_on, bool logout) PlayerMenu()
        {
            bool keepOn;
            bool logout;
            do
            {
                var reply = MainMenuWindow();
                keepOn = reply.keep_on;
                logout = reply.logout;
                if (!keepOn || logout) break;
                if (reply.action is not null)
                {
                    (bool keep_on, bool logout, bool back, SomeAction action) stop = reply.action.Invoke();
                    keepOn = stop.keep_on;
                    logout = stop.logout;
                }
            } while (!logout && keepOn);
            return (keepOn, logout);
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
                _memoryBools[i] = (false, false, false, false);
            }
        }
    }
}
