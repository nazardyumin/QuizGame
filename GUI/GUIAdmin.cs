using NStack;
using QuizGame.Features;
using QuizGame.Helpers;
using QuizGame.Users;
using Terminal.Gui;

namespace QuizGame.GUI
{
    public class GuiAdmin : GuiDefault
    {
        protected QuizCreator _creator;
        protected int _iterator;
        protected string? _memory_question;
        protected List<(string, int)>? _memory_answers;
        protected string? _buffer_theme;
        protected int _buffer_level;
        protected string? _buffer_question;
        protected List<(string, int)>? _buffer_answers;
        protected bool _is_editing;
        protected bool _is_adding;
        protected bool _is_cancelling;
        public GuiAdmin(User user) : base(user)
        {
            _creator = new(user);
            _changePass = _creator.ChangePassword;
            _changeDate = _creator.ChangeDateOfBirth;
            _iterator = 0;
            _buffer_theme = "";
            _buffer_level = 0;
            _buffer_question = "";
            _buffer_answers = new List<(string, int)>() { ("", 0), ("", 0), ("", 0), ("", 0), };
            _is_editing = false;
            _is_adding = false;
            _is_cancelling = false;
        }
        protected (bool keep_on, bool logout, bool back, SomeAction action) MainMenuWindow()
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
             new MenuItem ("_Logout", "", () => { logout=true; top.Running = false; }),
             new MenuItem("_Quit", "", () => {  if (GuiHelper.Quit()) {keep_on = false;top.Running = false; } }) })});
            top.Add(menu);
            var hello = new Label(_role)
            {
                X = Pos.AnchorEnd(_role.Length) - 1,
                Y = Pos.AnchorEnd(1),
                ColorScheme = Colors.Menu
            };
            win.Add(hello);
            var create_quiz = new Button("Create New Quiz")
            {
                X = Pos.Center(),
                Y = 5
            };
            create_quiz.Clicked += () =>
            {
                action = CreateQuizWindow;
                top.Running = false;
            };
            var edit_quiz = new Button("Edit Quiz")
            {
                X = Pos.Center(),
                Y = Pos.Top(create_quiz) + 2
            };
            edit_quiz.Clicked += () =>
            {
                action = EditQuizWindow;
                top.Running = false;
            };
            win.Add(create_quiz, edit_quiz);
            Application.Run();
            return (keep_on!, logout!, back!, action!);
        }
        protected (bool keep_on, bool logout, bool back, SomeAction action) CreateQuizWindow()
        {
            bool logout;
            bool keep_on;
            bool back;
            SomeAction? action = null;
            (bool keep_on, bool logout, bool back, SomeAction action) stop;
            do
            {
                stop = CreateQuizAddingItems();
                logout = stop.logout;
                keep_on = stop.keep_on;
                back = stop.back;
            } while (!logout && keep_on && !back);
            return (keep_on!, logout!, back!, action!);
        }
        protected (bool keep_on, bool logout, bool back, SomeAction action) CreateQuizAddingItems()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction? action = null;
            Application.Init();
            int count = _creator.GetCount();
            int our_position = 0;
            int zero_position = our_position - count;
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
             new MenuItem("_Quit", "", () => {  if (GuiHelper.Quit()) {keep_on = false; top.Running = false;} }) })});
            top.Add(menu);
            var hello = new Label(_role)
            {
                X = Pos.AnchorEnd(_role.Length) - 1,
                Y = Pos.AnchorEnd(1),
                ColorScheme = Colors.Menu
            };
            win.Add(hello);
            var header = new Label("Creating New Quiz: ")
            {
                X = Pos.Center(),
                Y = 2
            };
            var theme = new Label("Theme: ")
            {
                X = 16,
                Y = Pos.Top(header) + 4
            };
            var theme_text = new TextField(_buffer_theme)
            {
                X = Pos.Left(theme) + 8,
                Y = Pos.Top(theme),
                Width = 20
            };
            var level = new Label("Level: ")
            {
                X = Pos.Left(theme),
                Y = Pos.Top(theme) + 3
            };
            var levels = new RadioGroup(new ustring[] { "Easy", "Normal", "Hard" }, 0)
            {
                X = Pos.Right(level) + 1,
                Y = Pos.Top(theme) + 2,
                SelectedItem = _buffer_level
            };
            if (count > 0 && _buffer_theme != "")
            {
                theme_text.CanFocus = false;
                levels.CanFocus = false;
            }
            var question = new Label($"Question {count + _iterator + 1}: ")
            {
                Y = Pos.Top(level) + 4
            };
            if (count + 1 >= 10)
            {
                question.X = Pos.Left(level) - 6;
            }
            else
            {
                question.X = Pos.Left(level) - 5;
            }
            var question_text = new TextField(_buffer_question)
            {
                X = Pos.Left(levels),
                Y = Pos.Top(question),
                Width = 80
            };
            var answer1 = new Label("Answer 1: ")
            {
                X = Pos.Left(question) + 2,
                Y = Pos.Top(question) + 3
            };
            var answer1_text = new TextField(_buffer_answers![0].Item1)
            {
                X = Pos.Left(levels),
                Y = Pos.Top(answer1),
                Width = 40
            };
            var is_correct1 = new RadioGroup(new ustring[] { "Incorrect", "Correct" })
            {
                X = Pos.Right(answer1_text) + 2,
                Y = Pos.Top(question) + 3,
                SelectedItem = _buffer_answers[0].Item2
            };
            var answer2 = new Label("Answer 2: ")
            {
                X = Pos.Left(answer1),
                Y = Pos.Top(answer1) + 3
            };
            var answer2_text = new TextField(_buffer_answers![1].Item1)
            {
                X = Pos.Left(levels),
                Y = Pos.Top(answer1) + 3,
                Width = 40
            };
            var is_correct2 = new RadioGroup(new ustring[] { "Incorrect", "Correct" })
            {
                X = Pos.Left(is_correct1),
                Y = Pos.Top(is_correct1) + 3,
                SelectedItem = _buffer_answers[1].Item2
            };
            var answer3 = new Label("Answer 3: ")
            {
                X = Pos.Left(answer1),
                Y = Pos.Top(answer2) + 3
            };
            var answer3_text = new TextField(_buffer_answers![2].Item1)
            {
                X = Pos.Left(levels),
                Y = Pos.Top(answer2) + 3,
                Width = 40
            };
            var is_correct3 = new RadioGroup(new ustring[] { "Incorrect", "Correct" })
            {
                X = Pos.Left(is_correct1),
                Y = Pos.Top(is_correct2) + 3,
                SelectedItem = _buffer_answers[2].Item2
            };
            var answer4 = new Label("Answer 4: ")
            {
                X = Pos.Left(answer1),
                Y = Pos.Top(answer3) + 3
            };
            var answer4_text = new TextField(_buffer_answers![3].Item1)
            {
                X = Pos.Left(levels),
                Y = Pos.Top(answer3) + 3,
                Width = 40
            };
            var is_correct4 = new RadioGroup(new ustring[] { "Incorrect", "Correct" })
            {
                X = Pos.Left(is_correct1),
                Y = Pos.Top(is_correct3) + 3,
                SelectedItem = _buffer_answers[3].Item2
            };
            var add_item = new Button("Add Item")
            {
                X = Pos.Center(),
                Y = Pos.Top(is_correct4) + 6
            };
            var step_back = new Button("Back")
            {
                X = Pos.Left(add_item) - 10,
                Y = Pos.Top(is_correct4) + 6
            };
            var step_forward = new Button("Forward")
            {
                X = Pos.Right(add_item) + 2,
                Y = Pos.Top(is_correct4) + 6
            };
            var edit_item = new Button("Edit Item")
            {
                X = Pos.Center(),
                Y = Pos.Top(is_correct4) + 6
            };
            var fast_forward = new Button(">>>")
            {
                X = Pos.Left(step_forward) + 13,
                Y = Pos.Top(is_correct4) + 6
            }; var fast_back = new Button("<<<")
            {
                X = Pos.Left(step_back) - 9,
                Y = Pos.Top(is_correct4) + 6
            };
            var questions_added = new Label($"Questions added: {count}")
            {
                Y = Pos.Top(header) + 4
            };
            if (count < 10)
            {
                questions_added.X = Pos.AnchorEnd(32);
            }
            else
            {
                questions_added.X = Pos.AnchorEnd(33);
            }
            var cancel = new Button("Cancel")
            {
                X = Pos.Center() - 11,
                Y = Pos.Top(add_item) + 4
            };
            cancel.Clicked += () =>
            {
                _buffer_question = "";
                for (int j = 0; j < 4; j++)
                {
                    _buffer_answers[j] = ("", 0);
                }
                _creator.Clear();
                _buffer_level = 0;
                _buffer_theme = "";
                _iterator = 0;
                back = true;
                top.Running = false;
            };
            var save = new Button("Save")
            {
                X = Pos.Right(cancel) + 2,
                Y = Pos.Top(add_item) + 4
            };
            save.Clicked += () =>
            {
                if (theme_text.Text == "")
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "Theme is not specified!", "Ok");
                    top.Running = false;
                }
                else
                {
                    _buffer_theme = theme_text.Text.ToString();
                    var checker = _creator.FindQuiz(_buffer_theme!);
                    string existing_quiz = "";
                    if (checker is not null)
                    {
                        existing_quiz = $"{checker.Theme}{checker.Level}";
                    }
                    if (existing_quiz == $"{_buffer_theme}{_buffer_level}")
                    {
                        MessageBox.ErrorQuery(30, 7, "Error!", "Sorry! A quiz with these theme and level exists!\nPlease specify a unique name!", "Ok");
                        theme_text.CanFocus = true;
                        levels.CanFocus = true;
                        _buffer_theme = "";
                        top.Running = false;
                    }
                    if (GuiHelper.Save())
                    {
                        _creator.SetTheme(_buffer_theme!);
                        string what_level = "";
                        switch (_buffer_level)
                        {
                            case 0:
                                what_level = "Easy";
                                break;
                            case 1:
                                what_level = "Normal";
                                break;
                            case 2:
                                what_level = "Hard";
                                break;
                            default:
                                break;
                        }
                        _creator.SetLevel(what_level);
                        QuizSaver.ToFile(_creator.GetQuiz());
                        _buffer_theme = "";
                        _buffer_level = 0;
                        _buffer_question = "";
                        for (int j = 0; j < 4; j++)
                        {
                            _buffer_answers[j] = ("", 0);
                        }
                        _iterator = 0;
                        _creator.Clear();
                        back = true;
                        top.Running = false;
                    }
                    else
                    {
                        top.Running = false;
                    }
                }
            };
            if (_iterator + 1 < 0)
            {
                fast_forward.Visible = true;
            }
            else
            {
                fast_forward.Visible = false;
            }
            if (count > 1 && _iterator - 1 > zero_position)
            {
                fast_back.Visible = true;
            }
            else
            {
                fast_back.Visible = false;
            }
            if (count == 0)
            {
                edit_item.Visible = false;
                add_item.Visible = true;
                step_back.Visible = false;
                step_forward.Visible = false;
            }
            else if (_iterator == zero_position && count > 0)
            {
                add_item.Visible = false;
                edit_item.Visible = true;
                step_back.Visible = false;
                step_forward.Visible = true;
            }
            else if (_iterator > zero_position && _iterator < 0)
            {
                add_item.Visible = false;
                edit_item.Visible = true;
                step_back.X = Pos.Right(edit_item) - 23;
                step_back.Visible = true;
                step_forward.Visible = true;
            }
            else
            {
                edit_item.Visible = false;
                add_item.Visible = true;
                step_back.Visible = true;
                step_forward.Visible = false;
            }
            fast_forward.Clicked += () =>
            {
                _buffer_theme = theme_text.Text.ToString();
                _buffer_level = levels.SelectedItem;
                _iterator = 0;
                RestoreInputData();
                top.Running = false;
            };
            fast_back.Clicked += () =>
            {
                if (_iterator == 0) RememberInputData(question_text.Text.ToString()!, answer1_text.Text.ToString()!, answer2_text.Text.ToString()!, answer3_text.Text.ToString()!, answer4_text.Text.ToString()!,
                                 is_correct1.SelectedItem, is_correct2.SelectedItem, is_correct3.SelectedItem, is_correct4.SelectedItem);
                _iterator = zero_position;
                int i = _creator.GetCount() + _iterator;
                _buffer_question = _creator.GetQuestion(i);
                for (int j = 0; j < 4; j++)
                {
                    _buffer_answers[j] = _creator.GetAnswer(i, j);
                }
                top.Running = false;
            };
            edit_item.Clicked += () =>
            {
                int i = _creator.GetCount() + _iterator;
                _creator.EditQuestion(question_text.Text.ToString()!, i);
                _creator.EditAnswer(answer1_text.Text.ToString()!, is_correct1.SelectedItem, i, 0);
                _creator.EditAnswer(answer2_text.Text.ToString()!, is_correct2.SelectedItem, i, 1);
                _creator.EditAnswer(answer3_text.Text.ToString()!, is_correct3.SelectedItem, i, 2);
                _creator.EditAnswer(answer4_text.Text.ToString()!, is_correct4.SelectedItem, i, 3);
                _buffer_question = _creator.GetQuestion(i);
                for (int j = 0; j < 4; j++)
                {
                    _buffer_answers[j] = _creator.GetAnswer(i, j);
                }
                top.Running = false;
            };
            add_item.Clicked += () =>
            {
                if (question_text.Text == "" || answer1_text.Text == "" || answer2_text.Text == "" || answer3_text.Text == "" || answer4_text.Text == "")
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "Not all fields are filled in!", "Ok");
                }
                else if (is_correct1.SelectedItem == 0 && is_correct2.SelectedItem == 0 && is_correct3.SelectedItem == 0 && is_correct4.SelectedItem == 0)
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "Correct answer is not chosen!", "Ok");
                }
                else
                {
                    _creator.SetQuestion(question_text.Text.ToString()!);
                    _creator.SetAnswer(answer1_text.Text.ToString()!, is_correct1.SelectedItem);
                    _creator.SetAnswer(answer2_text.Text.ToString()!, is_correct2.SelectedItem);
                    _creator.SetAnswer(answer3_text.Text.ToString()!, is_correct3.SelectedItem);
                    _creator.SetAnswer(answer4_text.Text.ToString()!, is_correct4.SelectedItem);
                    _creator.AddItem();
                    _buffer_question = "";
                    for (int j = 0; j < 4; j++)
                    {
                        _buffer_answers[j] = ("", 0);
                    }
                    _buffer_theme = theme_text.Text.ToString();
                    _buffer_level = levels.SelectedItem;
                    top.Running = false;
                }
            };
            step_back.Clicked += () =>
            {
                _buffer_theme = theme_text.Text.ToString();
                _buffer_level = levels.SelectedItem;
                if (_iterator == 0) RememberInputData(question_text.Text.ToString()!, answer1_text.Text.ToString()!, answer2_text.Text.ToString()!, answer3_text.Text.ToString()!, answer4_text.Text.ToString()!,
                                  is_correct1.SelectedItem, is_correct2.SelectedItem, is_correct3.SelectedItem, is_correct4.SelectedItem);
                Step('-');
                top.Running = false;
            };
            step_forward.Clicked += () =>
            {
                _buffer_theme = theme_text.Text.ToString();
                _buffer_level = levels.SelectedItem;
                if (_iterator + 1 == 0)
                {
                    _iterator = 0;
                    RestoreInputData();
                }
                else Step('+');
                top.Running = false;
            };
            win.Add(header, theme, theme_text, level, levels, question, question_text, answer1, answer1_text, is_correct1, answer2, answer2_text, is_correct2,
                    answer3, answer3_text, is_correct3, answer4, answer4_text, is_correct4, add_item, step_back, edit_item, step_forward, fast_forward, questions_added, cancel, save, fast_back);
            Application.Run();
            return (keep_on!, logout!, back!, action!);
        }
        protected (bool keep_on, bool logout, bool back, SomeAction action) EditQuizWindow()
        {
            bool logout;
            bool keep_on;
            bool back;
            SomeAction? action = null;
            (bool keep_on, bool logout, bool back, SomeAction action) stop;
            do
            {
                stop = EditQuizMenu();
                logout = stop.logout;
                keep_on = stop.keep_on;
                back = stop.back;
            } while (!logout && keep_on && !back);
            return (keep_on!, logout!, back!, action!);
        }
        protected (bool keep_on, bool logout, bool back, SomeAction action) EditQuizMenu()
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
            var hello = new Label($"{_role}")
            {
                X = Pos.AnchorEnd(_role.Length) - 1,
                Y = Pos.AnchorEnd(1),
                ColorScheme = Colors.Menu
            };
            win.Add(hello);
            if (!_is_editing)
            {
                var header = new Label("Available Quizes: ")
                {
                    X = Pos.Center(),
                    Y = 2
                };
                if (_creator.GetAllQuizThemes().Count == 0)
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
                    var buffer = new ustring[_creator.GetAllQuizThemes().Count];
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = _creator.GetAllQuizThemes()[i];
                    }
                    var themes_list = new RadioGroup(buffer)
                    {
                        X = Pos.Center(),
                        Y = Pos.Top(header) + 3
                    };
                    var cancel = new Button("Cancel")
                    {
                        X = Pos.Center() - 11,
                        Y = Pos.Bottom(themes_list) + 3
                    };
                    cancel.Clicked += () =>
                    {
                        back = true;
                        top.Running = false;
                    };
                    var edit = new Button("Edit")
                    {
                        X = Pos.Right(cancel) + 2,
                        Y = Pos.Top(cancel)
                    };
                    edit.Clicked += () =>
                    {
                        _is_editing = true;
                        _creator.QuizInit(_creator.FindQuiz(themes_list.SelectedItem)!);
                        top.Running = false;
                    };
                    win.Add(header, themes_list, edit, cancel);
                }
            }
            else
            {
                count = _creator.GetCount();
                if (_is_adding)
                {
                    _buffer_question = "";
                    for (int j = 0; j < 4; j++)
                    {
                        _buffer_answers![j] = ("", 0);
                    }
                }
                else
                {
                    if (_is_cancelling)
                    {
                        RestoreInputData();
                        _is_cancelling = false;
                    }
                    else
                    {
                        _buffer_question = _creator.GetQuestion(_iterator);
                        _buffer_answers = _creator.GetAnswers(_iterator);
                    }
                }
                _buffer_theme = _creator.GetTheme();
                _buffer_level = _creator.GetLevelInt();
                var header = new Label("Editing Quiz: ")
                {
                    X = Pos.Center(),
                    Y = 2
                };
                var theme = new Label("Theme: ")
                {
                    X = 16,
                    Y = Pos.Top(header) + 4
                };
                var theme_text = new TextField(_buffer_theme)
                {
                    X = Pos.Left(theme) + 8,
                    Y = Pos.Top(theme),
                    Width = 20,
                    CanFocus = false
                };
                var level = new Label("Level: ")
                {
                    X = Pos.Left(theme),
                    Y = Pos.Top(theme) + 3
                };
                var levels = new RadioGroup(new ustring[] { "Easy", "Normal", "Hard" }, 0)
                {
                    X = Pos.Right(level) + 1,
                    Y = Pos.Top(theme) + 2,
                    SelectedItem = _buffer_level,
                    CanFocus = false
                };
                var question = new Label($"Question {_iterator + 1}: ")
                {
                    Y = Pos.Top(level) + 4
                };
                if (count + 1 >= 10)
                {
                    question.X = Pos.Left(level) - 6;
                }
                else
                {
                    question.X = Pos.Left(level) - 5;
                }
                var question_text = new TextField(_buffer_question)
                {
                    X = Pos.Left(levels),
                    Y = Pos.Top(question),
                    Width = 80
                };
                var answer1 = new Label("Answer 1: ")
                {
                    X = Pos.Left(question) + 2,
                    Y = Pos.Top(question) + 3
                };
                var answer1_text = new TextField(_buffer_answers![0].Item1)
                {
                    X = Pos.Left(levels),
                    Y = Pos.Top(answer1),
                    Width = 40
                };
                var is_correct1 = new RadioGroup(new ustring[] { "Incorrect", "Correct" })
                {
                    X = Pos.Right(answer1_text) + 2,
                    Y = Pos.Top(question) + 3,
                    SelectedItem = _buffer_answers[0].Item2
                };
                var answer2 = new Label("Answer 2: ")
                {
                    X = Pos.Left(answer1),
                    Y = Pos.Top(answer1) + 3
                };
                var answer2_text = new TextField(_buffer_answers[1].Item1)
                {
                    X = Pos.Left(levels),
                    Y = Pos.Top(answer1) + 3,
                    Width = 40
                };
                var is_correct2 = new RadioGroup(new ustring[] { "Incorrect", "Correct" })
                {
                    X = Pos.Left(is_correct1),
                    Y = Pos.Top(is_correct1) + 3,
                    SelectedItem = _buffer_answers[1].Item2
                };
                var answer3 = new Label("Answer 3: ")
                {
                    X = Pos.Left(answer1),
                    Y = Pos.Top(answer2) + 3
                };
                var answer3_text = new TextField(_buffer_answers[2].Item1)
                {
                    X = Pos.Left(levels),
                    Y = Pos.Top(answer2) + 3,
                    Width = 40
                };
                var is_correct3 = new RadioGroup(new ustring[] { "Incorrect", "Correct" })
                {
                    X = Pos.Left(is_correct1),
                    Y = Pos.Top(is_correct2) + 3,
                    SelectedItem = _buffer_answers[2].Item2
                };
                var answer4 = new Label("Answer 4: ")
                {
                    X = Pos.Left(answer1),
                    Y = Pos.Top(answer3) + 3
                };
                var answer4_text = new TextField(_buffer_answers[3].Item1)
                {
                    X = Pos.Left(levels),
                    Y = Pos.Top(answer3) + 3,
                    Width = 40
                };
                var is_correct4 = new RadioGroup(new ustring[] { "Incorrect", "Correct" })
                {
                    X = Pos.Left(is_correct1),
                    Y = Pos.Top(is_correct3) + 3,
                    SelectedItem = _buffer_answers[3].Item2
                };
                var add_item = new Button("Add Item");
                if (count == 0)
                {
                    add_item.X = Pos.Center() + 1;
                    add_item.Y = Pos.Top(is_correct4) + 6;
                    add_item.Clicked += () =>
                    {
                        if (question_text.Text == "" || answer1_text.Text == "" || answer2_text.Text == "" || answer3_text.Text == "" || answer4_text.Text == "")
                        {
                            MessageBox.ErrorQuery(30, 7, "Error!", "Not all fields are filled in!", "Ok");
                        }
                        else
                        {
                            _creator.SetQuestion(question_text.Text.ToString()!);
                            _creator.SetAnswer(answer1_text.Text.ToString()!, is_correct1.SelectedItem);
                            _creator.SetAnswer(answer2_text.Text.ToString()!, is_correct2.SelectedItem);
                            _creator.SetAnswer(answer3_text.Text.ToString()!, is_correct3.SelectedItem);
                            _creator.SetAnswer(answer4_text.Text.ToString()!, is_correct4.SelectedItem);
                            _creator.AddItem();
                            _creator.SetTheme(theme_text.Text.ToString()!);
                            string what_level = "";
                            switch (levels.SelectedItem)
                            {
                                case 0:
                                    what_level = "Easy";
                                    break;
                                case 1:
                                    what_level = "Normal";
                                    break;
                                case 2:
                                    what_level = "Hard";
                                    break;
                                default:
                                    break;
                            }
                            _creator.SetLevel(what_level);
                            for (int j = 0; j < 4; j++)
                            {
                                _buffer_answers[j] = _creator.GetAnswer(_iterator, j);
                            }
                            top.Running = false;
                        }
                    };
                }
                else
                {
                    add_item.X = Pos.Center() + 8;
                    add_item.Y = Pos.Top(is_correct4) + 6;
                    add_item.Clicked += () =>
                    {
                        _is_adding = true;
                        _iterator++;
                        top.Running = false;
                    };
                }
                var edit_item = new Button("Edit Item")
                {
                    X = Pos.Center(),
                    Y = Pos.Top(is_correct4) + 6
                };
                var step_back = new Button("Back")
                {
                    X = Pos.Left(edit_item) - 10,
                    Y = Pos.Top(is_correct4) + 6
                };
                var step_forward = new Button("Forward")
                {
                    X = Pos.Right(edit_item) + 2,
                    Y = Pos.Top(is_correct4) + 6
                };
                var fast_forward = new Button(">>>")
                {
                    X = Pos.Left(step_forward) + 13,
                    Y = Pos.Top(is_correct4) + 6
                }; var fast_back = new Button("<<<")
                {
                    X = Pos.Left(step_back) - 9,
                    Y = Pos.Top(is_correct4) + 6
                };
                var delete_item = new Button("Delete Item")
                {
                    X = Pos.Center(),
                    Y = Pos.Top(edit_item) + 2
                };
                delete_item.Clicked += () =>
                {
                    _creator.DeleteItem(_iterator);
                    if (_iterator == count - 1) _iterator--;
                    if (count == 1)
                    {
                        _iterator = 0;
                        _buffer_question = "";
                        for (int j = 0; j < 4; j++)
                        {
                            _buffer_answers[j] = ("", 0);
                        }
                    }
                    top.Running = false;
                };
                if (count == 0) delete_item.Visible = false;
                var questions_added = new Label($"Questions added: {count}")
                {
                    Y = Pos.Top(header) + 4
                };
                if (count < 10)
                {
                    questions_added.X = Pos.AnchorEnd(32);
                }
                else
                {
                    questions_added.X = Pos.AnchorEnd(33);
                }
                var cancel = new Button("Cancel")
                {
                    X = Pos.Center() - 11,
                    Y = Pos.Top(add_item) + 4
                };
                if (count == 0)
                {
                    cancel.X = Pos.Center() - 11;
                    cancel.Y = Pos.Top(add_item);
                }
                else
                {
                    cancel.X = Pos.Center() - 11;
                    cancel.Y = Pos.Top(add_item) + 4;
                }
                cancel.Clicked += () =>
                {
                    if (count == 0)
                    {
                        if (GuiHelper.Cancel())
                        {
                            _buffer_question = "";
                            for (int j = 0; j < 4; j++)
                            {
                                _buffer_answers[j] = ("", 0);
                            }
                            _creator.Clear();
                            _buffer_level = 0;
                            _buffer_theme = "";
                            _iterator = 0;
                            _is_editing = false;
                            top.Running = false;
                        }
                        else
                        {
                            RememberInputData(question_text.Text.ToString()!, answer1_text.Text.ToString()!, answer2_text.Text.ToString()!, answer3_text.Text.ToString()!, answer4_text.Text.ToString()!,
                                  is_correct1.SelectedItem, is_correct2.SelectedItem, is_correct3.SelectedItem, is_correct4.SelectedItem);
                            _is_cancelling = true;
                            top.Running = false;
                        }
                    }
                    else
                    {
                        _buffer_question = "";
                        for (int j = 0; j < 4; j++)
                        {
                            _buffer_answers[j] = ("", 0);
                        }
                        _creator.Clear();
                        _buffer_level = 0;
                        _buffer_theme = "";
                        _iterator = 0;
                        _is_editing = false;
                        top.Running = false;
                    }
                };
                var save = new Button("Save")
                {
                    X = Pos.Right(cancel) + 2,
                    Y = Pos.Top(add_item) + 4
                };
                save.Clicked += () =>
                {
                    if (GuiHelper.Save())
                    {
                        _creator.SetTheme(_buffer_theme);
                        string what_level = "";
                        switch (_buffer_level)
                        {
                            case 0:
                                what_level = "Easy";
                                break;
                            case 1:
                                what_level = "Normal";
                                break;
                            case 2:
                                what_level = "Hard";
                                break;
                            default:
                                break;
                        }
                        _creator.SetLevel(what_level);
                        QuizSaver.ReSaveToFile(_creator.GetQuiz());
                        _buffer_theme = "";
                        _buffer_level = 0;
                        _buffer_question = "";
                        for (int j = 0; j < 4; j++)
                        {
                            _buffer_answers[j] = ("", 0);
                        }
                        _iterator = 0;
                        _creator.Clear();
                        _is_editing = false;
                        back = true;
                        top.Running = false;
                    }
                    else
                    {
                        top.Running = false;
                    }
                };
                if (!_is_adding)
                {
                    if (count == 0)
                    {
                        fast_back.Visible = false;
                        step_back.Visible = false;
                        step_forward.Visible = false;
                        fast_forward.Visible = false;
                        add_item.Visible = true;
                        edit_item.Visible = false;
                        delete_item.Visible = false;
                        save.Visible = false;
                    }
                    else if (count == 1)
                    {
                        fast_back.Visible = false;
                        step_back.Visible = false;
                        step_forward.Visible = false;
                        fast_forward.Visible = false;
                        edit_item.Visible = true;
                        delete_item.Visible = true;
                        add_item.Visible = true;
                    }
                    else if (count == 2 && _iterator == 0)
                    {
                        fast_back.Visible = false;
                        step_back.Visible = false;
                        step_forward.Visible = true;
                        fast_forward.Visible = false;
                        edit_item.Visible = true;
                        delete_item.Visible = true;
                        add_item.Visible = false;
                    }
                    else if (count == 2 && _iterator == 1)
                    {
                        fast_back.Visible = false;
                        step_back.Visible = true;
                        step_forward.Visible = false;
                        fast_forward.Visible = false;
                        edit_item.Visible = true;
                        delete_item.Visible = true;
                        add_item.Visible = true;
                    }
                    else if (count >= 3 && _iterator == 0)
                    {
                        fast_back.Visible = false;
                        step_back.Visible = false;
                        step_forward.Visible = true;
                        fast_forward.Visible = true;
                        edit_item.Visible = true;
                        delete_item.Visible = true;
                        add_item.Visible = false;
                    }
                    else if (count == 3 && _iterator == 1)
                    {
                        fast_back.Visible = false;
                        step_back.Visible = true;
                        step_forward.Visible = true;
                        fast_forward.Visible = false;
                        edit_item.Visible = true;
                        delete_item.Visible = true;
                        add_item.Visible = false;
                    }
                    else if ((count == 3 && _iterator == 2) || (count > 3 && _iterator == count - 1))
                    {
                        fast_back.Visible = true;
                        step_back.Visible = true;
                        step_forward.Visible = false;
                        fast_forward.Visible = false;
                        edit_item.Visible = true;
                        delete_item.Visible = true;
                        add_item.Visible = true;
                    }
                    else if (count > 3 && _iterator == 1)
                    {
                        fast_back.Visible = false;
                        step_back.Visible = true;
                        step_forward.Visible = true;
                        fast_forward.Visible = true;
                        edit_item.Visible = true;
                        delete_item.Visible = true;
                        add_item.Visible = false;
                    }
                    else if (count > 3 && _iterator == count - 2)
                    {
                        fast_back.Visible = true;
                        step_back.Visible = true;
                        step_forward.Visible = true;
                        fast_forward.Visible = false;
                        edit_item.Visible = true;
                        delete_item.Visible = true;
                        add_item.Visible = false;
                    }
                    else
                    {
                        fast_back.Visible = true;
                        step_back.Visible = true;
                        step_forward.Visible = true;
                        fast_forward.Visible = true;
                        edit_item.Visible = true;
                        delete_item.Visible = true;
                        add_item.Visible = false;
                    }
                }
                else
                {
                    add_item.Visible = false;
                    fast_back.Visible = false;
                    step_back.Visible = false;
                    step_forward.Visible = false;
                    fast_forward.Visible = false;
                    edit_item.Visible = false;
                    delete_item.Visible = false;
                    save.Visible = false;
                    cancel.Visible = false;
                    var dont_add = new Button("Cancel")
                    {
                        X = Pos.Center() - 10,
                        Y = Pos.Top(add_item),
                    };
                    dont_add.Clicked += () =>
                    {
                        _is_adding = false;
                        _iterator--;
                        top.Running = false;
                    };
                    var add = new Button("Ok")
                    {
                        X = Pos.Right(dont_add) + 2,
                        Y = Pos.Top(add_item),
                    };
                    add.Clicked += () =>
                    {
                        if (question_text.Text == "" || answer1_text.Text == "" || answer2_text.Text == "" || answer3_text.Text == "" || answer4_text.Text == "")
                        {
                            MessageBox.ErrorQuery(30, 7, "Error!", "Not all fields are filled in!", "Ok");
                        }
                        else
                        {
                            _creator.SetQuestion(question_text.Text.ToString()!);
                            _creator.SetAnswer(answer1_text.Text.ToString()!, is_correct1.SelectedItem);
                            _creator.SetAnswer(answer2_text.Text.ToString()!, is_correct2.SelectedItem);
                            _creator.SetAnswer(answer3_text.Text.ToString()!, is_correct3.SelectedItem);
                            _creator.SetAnswer(answer4_text.Text.ToString()!, is_correct4.SelectedItem);
                            _creator.AddItem();
                            _creator.SetTheme(theme_text.Text.ToString()!);
                            string what_level = "";
                            switch (levels.SelectedItem)
                            {
                                case 0:
                                    what_level = "Easy";
                                    break;
                                case 1:
                                    what_level = "Normal";
                                    break;
                                case 2:
                                    what_level = "Hard";
                                    break;
                                default:
                                    break;
                            }
                            _creator.SetLevel(what_level);
                            for (int j = 0; j < 4; j++)
                            {
                                _buffer_answers[j] = _creator.GetAnswer(_iterator, j);
                            }
                            _is_adding = false;
                            top.Running = false;
                        }
                    };
                    win.Add(add, dont_add);
                }
                fast_forward.Clicked += () =>
                {
                    _iterator = count - 1;
                    top.Running = false;
                };
                fast_back.Clicked += () =>
                {
                    if (_iterator == count) RememberInputData(question_text.Text.ToString()!, answer1_text.Text.ToString()!, answer2_text.Text.ToString()!, answer3_text.Text.ToString()!, answer4_text.Text.ToString()!,
                                     is_correct1.SelectedItem, is_correct2.SelectedItem, is_correct3.SelectedItem, is_correct4.SelectedItem);
                    _iterator = 0;
                    top.Running = false;
                };
                edit_item.Clicked += () =>
                {
                    int i = _iterator;
                    _creator.SetTheme(theme_text.Text.ToString()!);
                    string what_level = "";
                    switch (levels.SelectedItem)
                    {
                        case 0:
                            what_level = "Easy";
                            break;
                        case 1:
                            what_level = "Normal";
                            break;
                        case 2:
                            what_level = "Hard";
                            break;
                        default:
                            break;
                    }
                    _creator.SetLevel(what_level);
                    _creator.EditQuestion(question_text.Text.ToString()!, i);
                    _creator.EditAnswer(answer1_text.Text.ToString()!, is_correct1.SelectedItem, i, 0);
                    _creator.EditAnswer(answer2_text.Text.ToString()!, is_correct2.SelectedItem, i, 1);
                    _creator.EditAnswer(answer3_text.Text.ToString()!, is_correct3.SelectedItem, i, 2);
                    _creator.EditAnswer(answer4_text.Text.ToString()!, is_correct4.SelectedItem, i, 3);
                    _buffer_question = _creator.GetQuestion(i);
                    for (int j = 0; j < 4; j++)
                    {
                        _buffer_answers[j] = _creator.GetAnswer(i, j);
                    }
                    top.Running = false;
                };
                step_back.Clicked += () =>
                {
                    _iterator--;
                    top.Running = false;
                };
                step_forward.Clicked += () =>
                {
                    _iterator++;
                    top.Running = false;
                };
                win.Add(header, theme, theme_text, level, levels, question, question_text, answer1, answer1_text, is_correct1, answer2, answer2_text, is_correct2,
                        answer3, answer3_text, is_correct3, answer4, answer4_text, is_correct4, add_item, step_back, edit_item, step_forward, fast_forward, questions_added, cancel, save, fast_back, delete_item);
            }
            Application.Run();
            return (keep_on!, logout!, back!, action!);
        }
        protected void Step(char key)
        {
            switch (key)
            {
                case '-':
                    _iterator -= 1;
                    int a = _creator.GetCount() + _iterator;
                    _buffer_question = _creator.GetQuestion(a);
                    for (int j = 0; j < 4; j++)
                    {
                        _buffer_answers![j] = _creator.GetAnswer(a, j);
                    }
                    break;
                case '+':
                    _iterator += 1;
                    int b = _creator.GetCount() + _iterator;
                    _buffer_question = _creator.GetQuestion(b);
                    for (int j = 0; j < 4; j++)
                    {
                        _buffer_answers![j] = _creator.GetAnswer(b, j);
                    }
                    break;
                default:
                    break;
            }
        }
        protected void RememberInputData(string q, string a1, string a2, string a3, string a4, int c1, int c2, int c3, int c4)
        {
            _memory_question = q;
            _memory_answers = new()
            {
                (a1, c1),
                (a2, c2),
                (a3, c3),
                (a4, c4)
            };
        }
        protected void RestoreInputData()
        {
            _buffer_question = _memory_question;
            for (int i = 0; i < 4; i++)
            {
                _buffer_answers![i] = (_memory_answers![i].Item1, _memory_answers[i].Item2);
            }
            _memory_answers!.Clear();
        }
        public virtual (bool keep_on, bool logout) AdminMenu()
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
    }
}
