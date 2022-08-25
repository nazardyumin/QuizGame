using NStack;
using QuizGame.Features;
using QuizGame.Helpers;
using QuizGame.Quizes.QuizResults;
using QuizGame.Users;
using Terminal.Gui;

namespace QuizGame.GUI
{
    public class GuiAdmin : GuiDefault
    {
        protected readonly QuizCreator _creator;
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
             new MenuItem ("_Logout", "", () => { logout=true; top.Running = false; }),
             new MenuItem("_Quit", "", () => {  if (GuiHelper.Quit()) {keepOn = false;top.Running = false; } }) })});
            top.Add(menu);
            var hello = new Label(_role)
            {
                X = Pos.AnchorEnd(_role.Length) - 1,
                Y = Pos.AnchorEnd(1),
                ColorScheme = Colors.Menu
            };
            win.Add(hello);
            var createQuiz = new Button("Create New Quiz")
            {
                X = Pos.Center(),
                Y = 5
            };
            createQuiz.Clicked += () =>
            {
                action = CreateQuizWindow;
                top.Running = false;
            };
            var editQuiz = new Button("Edit Quiz")
            {
                X = Pos.Center(),
                Y = Pos.Top(createQuiz) + 2
            };
            editQuiz.Clicked += () =>
            {
                action = EditQuizWindow;
                top.Running = false;
            };
            win.Add(createQuiz, editQuiz);
            Application.Run();
            return (keepOn, logout, back, action!);
        }
        protected (bool keep_on, bool logout, bool back, SomeAction action) CreateQuizWindow()
        {
            bool logout;
            bool keepOn;
            bool back;
            SomeAction? action = null;
            do
            {
                var stop = CreateQuizAddingItems();
                logout = stop.logout;
                keepOn = stop.keep_on;
                back = stop.back;
            } while (!logout && keepOn && !back);
            return (keepOn, logout, back, action!);
        }
        protected (bool keep_on, bool logout, bool back, SomeAction action) CreateQuizAddingItems()
        {
            bool logout = false;
            bool keepOn = true;
            bool back = false;
            SomeAction? action = null;
            Application.Init();
            int count = _creator.GetCount();
            int ourPosition = 0;
            int zeroPosition = ourPosition - count;
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
             new MenuItem("_Quit", "", () => {  if (GuiHelper.Quit()) {keepOn = false; top.Running = false;} }) })});
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
            var themeText = new TextField(_buffer_theme)
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
            var levels = new RadioGroup(new ustring[] { "Easy", "Normal", "Hard" })
            {
                X = Pos.Right(level) + 1,
                Y = Pos.Top(theme) + 2,
                SelectedItem = _buffer_level
            };
            if (count > 0 && _buffer_theme != "")
            {
                themeText.CanFocus = false;
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
            var questionText = new TextField(_buffer_question)
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
            var answer1Text = new TextField(_buffer_answers![0].Item1)
            {
                X = Pos.Left(levels),
                Y = Pos.Top(answer1),
                Width = 40
            };
            var isCorrect1 = new RadioGroup(new ustring[] { "Incorrect", "Correct" })
            {
                X = Pos.Right(answer1Text) + 2,
                Y = Pos.Top(question) + 3,
                SelectedItem = _buffer_answers[0].Item2
            };
            var answer2 = new Label("Answer 2: ")
            {
                X = Pos.Left(answer1),
                Y = Pos.Top(answer1) + 3
            };
            var answer2Text = new TextField(_buffer_answers![1].Item1)
            {
                X = Pos.Left(levels),
                Y = Pos.Top(answer1) + 3,
                Width = 40
            };
            var isCorrect2 = new RadioGroup(new ustring[] { "Incorrect", "Correct" })
            {
                X = Pos.Left(isCorrect1),
                Y = Pos.Top(isCorrect1) + 3,
                SelectedItem = _buffer_answers[1].Item2
            };
            var answer3 = new Label("Answer 3: ")
            {
                X = Pos.Left(answer1),
                Y = Pos.Top(answer2) + 3
            };
            var answer3Text = new TextField(_buffer_answers![2].Item1)
            {
                X = Pos.Left(levels),
                Y = Pos.Top(answer2) + 3,
                Width = 40
            };
            var isCorrect3 = new RadioGroup(new ustring[] { "Incorrect", "Correct" })
            {
                X = Pos.Left(isCorrect1),
                Y = Pos.Top(isCorrect2) + 3,
                SelectedItem = _buffer_answers[2].Item2
            };
            var answer4 = new Label("Answer 4: ")
            {
                X = Pos.Left(answer1),
                Y = Pos.Top(answer3) + 3
            };
            var answer4Text = new TextField(_buffer_answers![3].Item1)
            {
                X = Pos.Left(levels),
                Y = Pos.Top(answer3) + 3,
                Width = 40
            };
            var isCorrect4 = new RadioGroup(new ustring[] { "Incorrect", "Correct" })
            {
                X = Pos.Left(isCorrect1),
                Y = Pos.Top(isCorrect3) + 3,
                SelectedItem = _buffer_answers[3].Item2
            };
            var addItem = new Button("Add Item")
            {
                X = Pos.Center(),
                Y = Pos.Top(isCorrect4) + 6
            };
            var stepBack = new Button("Back")
            {
                X = Pos.Left(addItem) - 10,
                Y = Pos.Top(isCorrect4) + 6
            };
            var stepForward = new Button("Forward")
            {
                X = Pos.Right(addItem) + 2,
                Y = Pos.Top(isCorrect4) + 6
            };
            var editItem = new Button("Edit Item")
            {
                X = Pos.Center(),
                Y = Pos.Top(isCorrect4) + 6
            };
            var fastForward = new Button(">>>")
            {
                X = Pos.Left(stepForward) + 13,
                Y = Pos.Top(isCorrect4) + 6
            }; var fastBack = new Button("<<<")
            {
                X = Pos.Left(stepBack) - 9,
                Y = Pos.Top(isCorrect4) + 6
            };
            var questionsAdded = new Label($"Questions added: {count}")
            {
                Y = Pos.Top(header) + 4
            };
            questionsAdded.X = Pos.AnchorEnd(count < 10 ? 32 : 33);
            var cancel = new Button("Cancel")
            {
                X = Pos.Center() - 11,
                Y = Pos.Top(addItem) + 4
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
                Y = Pos.Top(addItem) + 4
            };
            save.Clicked += () =>
            {
                if (themeText.Text == "")
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "Theme is not specified!", "Ok");
                    top.Running = false;
                }
                else
                {
                    _buffer_theme = themeText.Text.ToString();
                    var checker = _creator.FindQuiz(_buffer_theme!);
                    string existingQuiz = "";
                    if (checker is not null)
                    {
                        existingQuiz = $"{checker.Theme}{checker.Level}";
                    }
                    if (existingQuiz == $"{_buffer_theme}{_buffer_level}")
                    {
                        MessageBox.ErrorQuery(30, 7, "Error!", "Sorry! A quiz with these theme and level exists!\nPlease specify a unique name!", "Ok");
                        themeText.CanFocus = true;
                        levels.CanFocus = true;
                        _buffer_theme = "";
                        top.Running = false;
                    }
                    if (GuiHelper.Save())
                    {
                        _creator.SetTheme(_buffer_theme!);
                        string whatLevel = "";
                        switch (_buffer_level)
                        {
                            case 0:
                                whatLevel = "Easy";
                                break;
                            case 1:
                                whatLevel = "Normal";
                                break;
                            case 2:
                                whatLevel = "Hard";
                                break;
                        }
                        _creator.SetLevel(whatLevel);
                        QuizSaver.ToFile(_creator.GetQuiz());
                        Top20Serializer.CreateTop20File($"{_creator.GetTheme()} ({_creator.GetLevel()})");
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
            fastForward.Visible = _iterator + 1 < 0;
            if (count > 1 && _iterator - 1 > zeroPosition)
            {
                fastBack.Visible = true;
            }
            else
            {
                fastBack.Visible = false;
            }
            if (count == 0)
            {
                editItem.Visible = false;
                addItem.Visible = true;
                stepBack.Visible = false;
                stepForward.Visible = false;
            }
            else if (_iterator == zeroPosition && count > 0)
            {
                addItem.Visible = false;
                editItem.Visible = true;
                stepBack.Visible = false;
                stepForward.Visible = true;
            }
            else if (_iterator > zeroPosition && _iterator < 0)
            {
                addItem.Visible = false;
                editItem.Visible = true;
                stepBack.X = Pos.Right(editItem) - 23;
                stepBack.Visible = true;
                stepForward.Visible = true;
            }
            else
            {
                editItem.Visible = false;
                addItem.Visible = true;
                stepBack.Visible = true;
                stepForward.Visible = false;
            }
            fastForward.Clicked += () =>
            {
                _buffer_theme = themeText.Text.ToString();
                _buffer_level = levels.SelectedItem;
                _iterator = 0;
                RestoreInputData();
                top.Running = false;
            };
            fastBack.Clicked += () =>
            {
                if (_iterator == 0) RememberInputData(questionText.Text.ToString()!, answer1Text.Text.ToString()!, answer2Text.Text.ToString()!, answer3Text.Text.ToString()!, answer4Text.Text.ToString()!,
                                 isCorrect1.SelectedItem, isCorrect2.SelectedItem, isCorrect3.SelectedItem, isCorrect4.SelectedItem);
                _iterator = zeroPosition;
                int i = _creator.GetCount() + _iterator;
                _buffer_question = _creator.GetQuestion(i);
                for (int j = 0; j < 4; j++)
                {
                    _buffer_answers[j] = _creator.GetAnswer(i, j);
                }
                top.Running = false;
            };
            editItem.Clicked += () =>
            {
                int i = _creator.GetCount() + _iterator;
                _creator.EditQuestion(questionText.Text.ToString()!, i);
                _creator.EditAnswer(answer1Text.Text.ToString()!, isCorrect1.SelectedItem, i, 0);
                _creator.EditAnswer(answer2Text.Text.ToString()!, isCorrect2.SelectedItem, i, 1);
                _creator.EditAnswer(answer3Text.Text.ToString()!, isCorrect3.SelectedItem, i, 2);
                _creator.EditAnswer(answer4Text.Text.ToString()!, isCorrect4.SelectedItem, i, 3);
                _buffer_question = _creator.GetQuestion(i);
                for (int j = 0; j < 4; j++)
                {
                    _buffer_answers[j] = _creator.GetAnswer(i, j);
                }
                top.Running = false;
            };
            addItem.Clicked += () =>
            {
                if (questionText.Text == "" || answer1Text.Text == "" || answer2Text.Text == "" || answer3Text.Text == "" || answer4Text.Text == "")
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "Not all fields are filled in!", "Ok");
                }
                else if (isCorrect1.SelectedItem == 0 && isCorrect2.SelectedItem == 0 && isCorrect3.SelectedItem == 0 && isCorrect4.SelectedItem == 0)
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "Correct answer is not chosen!", "Ok");
                }
                else
                {
                    _creator.SetQuestion(questionText.Text.ToString()!);
                    _creator.SetAnswer(answer1Text.Text.ToString()!, isCorrect1.SelectedItem);
                    _creator.SetAnswer(answer2Text.Text.ToString()!, isCorrect2.SelectedItem);
                    _creator.SetAnswer(answer3Text.Text.ToString()!, isCorrect3.SelectedItem);
                    _creator.SetAnswer(answer4Text.Text.ToString()!, isCorrect4.SelectedItem);
                    _creator.AddItem();
                    _buffer_question = "";
                    for (int j = 0; j < 4; j++)
                    {
                        _buffer_answers[j] = ("", 0);
                    }
                    _buffer_theme = themeText.Text.ToString();
                    _buffer_level = levels.SelectedItem;
                    top.Running = false;
                }
            };
            stepBack.Clicked += () =>
            {
                _buffer_theme = themeText.Text.ToString();
                _buffer_level = levels.SelectedItem;
                if (_iterator == 0) RememberInputData(questionText.Text.ToString()!, answer1Text.Text.ToString()!, answer2Text.Text.ToString()!, answer3Text.Text.ToString()!, answer4Text.Text.ToString()!,
                                  isCorrect1.SelectedItem, isCorrect2.SelectedItem, isCorrect3.SelectedItem, isCorrect4.SelectedItem);
                Step('-');
                top.Running = false;
            };
            stepForward.Clicked += () =>
            {
                _buffer_theme = themeText.Text.ToString();
                _buffer_level = levels.SelectedItem;
                if (_iterator + 1 == 0)
                {
                    _iterator = 0;
                    RestoreInputData();
                }
                else Step('+');
                top.Running = false;
            };
            win.Add(header, theme, themeText, level, levels, question, questionText, answer1, answer1Text, isCorrect1, answer2, answer2Text, isCorrect2,
                    answer3, answer3Text, isCorrect3, answer4, answer4Text, isCorrect4, addItem, stepBack, editItem, stepForward, fastForward, questionsAdded, cancel, save, fastBack);
            Application.Run();
            return (keepOn, logout, back, action!);
        }
        protected (bool keep_on, bool logout, bool back, SomeAction action) EditQuizWindow()
        {
            bool logout;
            bool keepOn;
            bool back;
            SomeAction? action = null;
            do
            {
                var stop = EditQuizMenu();
                logout = stop.logout;
                keepOn = stop.keep_on;
                back = stop.back;
            } while (!logout && keepOn && !back);
            return (keepOn, logout, back, action!);
        }
        protected (bool keep_on, bool logout, bool back, SomeAction action) EditQuizMenu()
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
                    var buffer = new ustring[_creator.GetAllQuizThemes().Count];
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = _creator.GetAllQuizThemes()[i];
                    }
                    var themesList = new RadioGroup(buffer)
                    {
                        X = Pos.Center(),
                        Y = Pos.Top(header) + 3
                    };
                    var cancel = new Button("Cancel")
                    {
                        X = Pos.Center() - 11,
                        Y = Pos.Bottom(themesList) + 3
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
                        _creator.QuizInit(_creator.FindQuiz(themesList.SelectedItem));
                        top.Running = false;
                    };
                    win.Add(header, themesList, edit, cancel);
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
                var themeText = new TextField(_buffer_theme)
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
                var levels = new RadioGroup(new ustring[] { "Easy", "Normal", "Hard" })
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
                var questionText = new TextField(_buffer_question)
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
                var answer1Text = new TextField(_buffer_answers![0].Item1)
                {
                    X = Pos.Left(levels),
                    Y = Pos.Top(answer1),
                    Width = 40
                };
                var isCorrect1 = new RadioGroup(new ustring[] { "Incorrect", "Correct" })
                {
                    X = Pos.Right(answer1Text) + 2,
                    Y = Pos.Top(question) + 3,
                    SelectedItem = _buffer_answers[0].Item2
                };
                var answer2 = new Label("Answer 2: ")
                {
                    X = Pos.Left(answer1),
                    Y = Pos.Top(answer1) + 3
                };
                var answer2Text = new TextField(_buffer_answers[1].Item1)
                {
                    X = Pos.Left(levels),
                    Y = Pos.Top(answer1) + 3,
                    Width = 40
                };
                var isCorrect2 = new RadioGroup(new ustring[] { "Incorrect", "Correct" })
                {
                    X = Pos.Left(isCorrect1),
                    Y = Pos.Top(isCorrect1) + 3,
                    SelectedItem = _buffer_answers[1].Item2
                };
                var answer3 = new Label("Answer 3: ")
                {
                    X = Pos.Left(answer1),
                    Y = Pos.Top(answer2) + 3
                };
                var answer3Text = new TextField(_buffer_answers[2].Item1)
                {
                    X = Pos.Left(levels),
                    Y = Pos.Top(answer2) + 3,
                    Width = 40
                };
                var isCorrect3 = new RadioGroup(new ustring[] { "Incorrect", "Correct" })
                {
                    X = Pos.Left(isCorrect1),
                    Y = Pos.Top(isCorrect2) + 3,
                    SelectedItem = _buffer_answers[2].Item2
                };
                var answer4 = new Label("Answer 4: ")
                {
                    X = Pos.Left(answer1),
                    Y = Pos.Top(answer3) + 3
                };
                var answer4Text = new TextField(_buffer_answers[3].Item1)
                {
                    X = Pos.Left(levels),
                    Y = Pos.Top(answer3) + 3,
                    Width = 40
                };
                var isCorrect4 = new RadioGroup(new ustring[] { "Incorrect", "Correct" })
                {
                    X = Pos.Left(isCorrect1),
                    Y = Pos.Top(isCorrect3) + 3,
                    SelectedItem = _buffer_answers[3].Item2
                };
                var addItem = new Button("Add Item");
                if (count == 0)
                {
                    addItem.X = Pos.Center() + 1;
                    addItem.Y = Pos.Top(isCorrect4) + 6;
                    addItem.Clicked += () =>
                    {
                        if (questionText.Text == "" || answer1Text.Text == "" || answer2Text.Text == "" || answer3Text.Text == "" || answer4Text.Text == "")
                        {
                            MessageBox.ErrorQuery(30, 7, "Error!", "Not all fields are filled in!", "Ok");
                        }
                        else
                        {
                            _creator.SetQuestion(questionText.Text.ToString()!);
                            _creator.SetAnswer(answer1Text.Text.ToString()!, isCorrect1.SelectedItem);
                            _creator.SetAnswer(answer2Text.Text.ToString()!, isCorrect2.SelectedItem);
                            _creator.SetAnswer(answer3Text.Text.ToString()!, isCorrect3.SelectedItem);
                            _creator.SetAnswer(answer4Text.Text.ToString()!, isCorrect4.SelectedItem);
                            _creator.AddItem();
                            _creator.SetTheme(themeText.Text.ToString()!);
                            string whatLevel = "";
                            switch (levels.SelectedItem)
                            {
                                case 0:
                                    whatLevel = "Easy";
                                    break;
                                case 1:
                                    whatLevel = "Normal";
                                    break;
                                case 2:
                                    whatLevel = "Hard";
                                    break;
                            }
                            _creator.SetLevel(whatLevel);
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
                    addItem.X = Pos.Center() + 8;
                    addItem.Y = Pos.Top(isCorrect4) + 6;
                    addItem.Clicked += () =>
                    {
                        _is_adding = true;
                        _iterator++;
                        top.Running = false;
                    };
                }
                var editItem = new Button("Edit Item")
                {
                    X = Pos.Center(),
                    Y = Pos.Top(isCorrect4) + 6
                };
                var stepBack = new Button("Back")
                {
                    X = Pos.Left(editItem) - 10,
                    Y = Pos.Top(isCorrect4) + 6
                };
                var stepForward = new Button("Forward")
                {
                    X = Pos.Right(editItem) + 2,
                    Y = Pos.Top(isCorrect4) + 6
                };
                var fastForward = new Button(">>>")
                {
                    X = Pos.Left(stepForward) + 13,
                    Y = Pos.Top(isCorrect4) + 6
                }; var fastBack = new Button("<<<")
                {
                    X = Pos.Left(stepBack) - 9,
                    Y = Pos.Top(isCorrect4) + 6
                };
                var deleteItem = new Button("Delete Item")
                {
                    X = Pos.Center(),
                    Y = Pos.Top(editItem) + 2
                };
                deleteItem.Clicked += () =>
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
                if (count == 0) deleteItem.Visible = false;
                var questionsAdded = new Label($"Questions added: {count}")
                {
                    Y = Pos.Top(header) + 4
                };
                questionsAdded.X = Pos.AnchorEnd(count < 10 ? 32 : 33);
                var cancel = new Button("Cancel")
                {
                    X = Pos.Center() - 11,
                    Y = Pos.Top(addItem) + 4
                };
                if (count == 0)
                {
                    cancel.X = Pos.Center() - 11;
                    cancel.Y = Pos.Top(addItem);
                }
                else
                {
                    cancel.X = Pos.Center() - 11;
                    cancel.Y = Pos.Top(addItem) + 4;
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
                            RememberInputData(questionText.Text.ToString()!, answer1Text.Text.ToString()!, answer2Text.Text.ToString()!, answer3Text.Text.ToString()!, answer4Text.Text.ToString()!,
                                  isCorrect1.SelectedItem, isCorrect2.SelectedItem, isCorrect3.SelectedItem, isCorrect4.SelectedItem);
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
                    Y = Pos.Top(addItem) + 4
                };
                save.Clicked += () =>
                {
                    if (GuiHelper.Save())
                    {
                        _creator.SetTheme(_buffer_theme);
                        string whatLevel = "";
                        switch (_buffer_level)
                        {
                            case 0:
                                whatLevel = "Easy";
                                break;
                            case 1:
                                whatLevel = "Normal";
                                break;
                            case 2:
                                whatLevel = "Hard";
                                break;
                        }
                        _creator.SetLevel(whatLevel);
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
                        fastBack.Visible = false;
                        stepBack.Visible = false;
                        stepForward.Visible = false;
                        fastForward.Visible = false;
                        addItem.Visible = true;
                        editItem.Visible = false;
                        deleteItem.Visible = false;
                        save.Visible = false;
                    }
                    else if (count == 1)
                    {
                        fastBack.Visible = false;
                        stepBack.Visible = false;
                        stepForward.Visible = false;
                        fastForward.Visible = false;
                        editItem.Visible = true;
                        deleteItem.Visible = true;
                        addItem.Visible = true;
                    }
                    else if (count == 2 && _iterator == 0)
                    {
                        fastBack.Visible = false;
                        stepBack.Visible = false;
                        stepForward.Visible = true;
                        fastForward.Visible = false;
                        editItem.Visible = true;
                        deleteItem.Visible = true;
                        addItem.Visible = false;
                    }
                    else if (count == 2 && _iterator == 1)
                    {
                        fastBack.Visible = false;
                        stepBack.Visible = true;
                        stepForward.Visible = false;
                        fastForward.Visible = false;
                        editItem.Visible = true;
                        deleteItem.Visible = true;
                        addItem.Visible = true;
                    }
                    else if (count >= 3 && _iterator == 0)
                    {
                        fastBack.Visible = false;
                        stepBack.Visible = false;
                        stepForward.Visible = true;
                        fastForward.Visible = true;
                        editItem.Visible = true;
                        deleteItem.Visible = true;
                        addItem.Visible = false;
                    }
                    else if (count == 3 && _iterator == 1)
                    {
                        fastBack.Visible = false;
                        stepBack.Visible = true;
                        stepForward.Visible = true;
                        fastForward.Visible = false;
                        editItem.Visible = true;
                        deleteItem.Visible = true;
                        addItem.Visible = false;
                    }
                    else if ((count == 3 && _iterator == 2) || (count > 3 && _iterator == count - 1))
                    {
                        fastBack.Visible = true;
                        stepBack.Visible = true;
                        stepForward.Visible = false;
                        fastForward.Visible = false;
                        editItem.Visible = true;
                        deleteItem.Visible = true;
                        addItem.Visible = true;
                    }
                    else if (count > 3 && _iterator == 1)
                    {
                        fastBack.Visible = false;
                        stepBack.Visible = true;
                        stepForward.Visible = true;
                        fastForward.Visible = true;
                        editItem.Visible = true;
                        deleteItem.Visible = true;
                        addItem.Visible = false;
                    }
                    else if (count > 3 && _iterator == count - 2)
                    {
                        fastBack.Visible = true;
                        stepBack.Visible = true;
                        stepForward.Visible = true;
                        fastForward.Visible = false;
                        editItem.Visible = true;
                        deleteItem.Visible = true;
                        addItem.Visible = false;
                    }
                    else
                    {
                        fastBack.Visible = true;
                        stepBack.Visible = true;
                        stepForward.Visible = true;
                        fastForward.Visible = true;
                        editItem.Visible = true;
                        deleteItem.Visible = true;
                        addItem.Visible = false;
                    }
                }
                else
                {
                    addItem.Visible = false;
                    fastBack.Visible = false;
                    stepBack.Visible = false;
                    stepForward.Visible = false;
                    fastForward.Visible = false;
                    editItem.Visible = false;
                    deleteItem.Visible = false;
                    save.Visible = false;
                    cancel.Visible = false;
                    var dontAdd = new Button("Cancel")
                    {
                        X = Pos.Center() - 10,
                        Y = Pos.Top(addItem),
                    };
                    dontAdd.Clicked += () =>
                    {
                        _is_adding = false;
                        _iterator--;
                        top.Running = false;
                    };
                    var add = new Button("Ok")
                    {
                        X = Pos.Right(dontAdd) + 2,
                        Y = Pos.Top(addItem),
                    };
                    add.Clicked += () =>
                    {
                        if (questionText.Text == "" || answer1Text.Text == "" || answer2Text.Text == "" || answer3Text.Text == "" || answer4Text.Text == "")
                        {
                            MessageBox.ErrorQuery(30, 7, "Error!", "Not all fields are filled in!", "Ok");
                        }
                        else
                        {
                            _creator.SetQuestion(questionText.Text.ToString()!);
                            _creator.SetAnswer(answer1Text.Text.ToString()!, isCorrect1.SelectedItem);
                            _creator.SetAnswer(answer2Text.Text.ToString()!, isCorrect2.SelectedItem);
                            _creator.SetAnswer(answer3Text.Text.ToString()!, isCorrect3.SelectedItem);
                            _creator.SetAnswer(answer4Text.Text.ToString()!, isCorrect4.SelectedItem);
                            _creator.AddItem();
                            _creator.SetTheme(themeText.Text.ToString()!);
                            string whatLevel = "";
                            switch (levels.SelectedItem)
                            {
                                case 0:
                                    whatLevel = "Easy";
                                    break;
                                case 1:
                                    whatLevel = "Normal";
                                    break;
                                case 2:
                                    whatLevel = "Hard";
                                    break;
                            }
                            _creator.SetLevel(whatLevel);
                            for (int j = 0; j < 4; j++)
                            {
                                _buffer_answers[j] = _creator.GetAnswer(_iterator, j);
                            }
                            _is_adding = false;
                            top.Running = false;
                        }
                    };
                    win.Add(add, dontAdd);
                }
                fastForward.Clicked += () =>
                {
                    _iterator = count - 1;
                    top.Running = false;
                };
                fastBack.Clicked += () =>
                {
                    if (_iterator == count) RememberInputData(questionText.Text.ToString()!, answer1Text.Text.ToString()!, answer2Text.Text.ToString()!, answer3Text.Text.ToString()!, answer4Text.Text.ToString()!,
                                     isCorrect1.SelectedItem, isCorrect2.SelectedItem, isCorrect3.SelectedItem, isCorrect4.SelectedItem);
                    _iterator = 0;
                    top.Running = false;
                };
                editItem.Clicked += () =>
                {
                    int i = _iterator;
                    _creator.SetTheme(themeText.Text.ToString()!);
                    string whatLevel = "";
                    switch (levels.SelectedItem)
                    {
                        case 0:
                            whatLevel = "Easy";
                            break;
                        case 1:
                            whatLevel = "Normal";
                            break;
                        case 2:
                            whatLevel = "Hard";
                            break;
                    }
                    _creator.SetLevel(whatLevel);
                    _creator.EditQuestion(questionText.Text.ToString()!, i);
                    _creator.EditAnswer(answer1Text.Text.ToString()!, isCorrect1.SelectedItem, i, 0);
                    _creator.EditAnswer(answer2Text.Text.ToString()!, isCorrect2.SelectedItem, i, 1);
                    _creator.EditAnswer(answer3Text.Text.ToString()!, isCorrect3.SelectedItem, i, 2);
                    _creator.EditAnswer(answer4Text.Text.ToString()!, isCorrect4.SelectedItem, i, 3);
                    _buffer_question = _creator.GetQuestion(i);
                    for (int j = 0; j < 4; j++)
                    {
                        _buffer_answers[j] = _creator.GetAnswer(i, j);
                    }
                    top.Running = false;
                };
                stepBack.Clicked += () =>
                {
                    _iterator--;
                    top.Running = false;
                };
                stepForward.Clicked += () =>
                {
                    _iterator++;
                    top.Running = false;
                };
                win.Add(header, theme, themeText, level, levels, question, questionText, answer1, answer1Text, isCorrect1, answer2, answer2Text, isCorrect2,
                        answer3, answer3Text, isCorrect3, answer4, answer4Text, isCorrect4, addItem, stepBack, editItem, stepForward, fastForward, questionsAdded, cancel, save, fastBack, deleteItem);
            }
            Application.Run();
            return (keepOn, logout, back, action!);
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
    }
}
