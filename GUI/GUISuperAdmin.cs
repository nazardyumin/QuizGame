using NStack;
using QuizGame.Features;
using QuizGame.Helpers;
using QuizGame.Users;
using Terminal.Gui;

namespace QuizGame.GUI
{
    public class GuiSuperAdmin : GuiAdmin
    {
        private readonly SuperQuizCreator _superCreator;
        public GuiSuperAdmin(User user) : base(user)
        {
            _superCreator = new(user);
            _changePass = _superCreator.ChangePassword;
            _changeDate = _superCreator.ChangeDateOfBirth;
        }
        private new (bool keep_on, bool logout, bool back, SomeAction action) MainMenuWindow()
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
            var deleteQuiz = new Button("Delete Quiz")
            {
                X = Pos.Center(),
                Y = Pos.Top(editQuiz) + 2
            };
            deleteQuiz.Clicked += () =>
            {
                action = DeleteQuizWindow;
                top.Running = false;
            };
            var createAdmin = new Button("Create New Admin")
            {
                X = Pos.Center(),
                Y = Pos.Top(deleteQuiz) + 2
            };
            createAdmin.Clicked += () =>
            {
                action = CreateAdminWindow;
                top.Running = false;
            };
            win.Add(createQuiz, editQuiz, deleteQuiz, createAdmin);
            Application.Run();
            return (keepOn, logout, back, action!);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) DeleteQuizWindow()
        {
            bool logout;
            bool keepOn;
            bool back;
            SomeAction? action = null;
            do
            {
                var stop = DeleteQuizMenu();
                logout = stop.logout;
                keepOn = stop.keep_on;
                back = stop.back;
            } while (!logout && keepOn && !back);
            return (keepOn, logout, back, action!);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) DeleteQuizMenu()
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
            var hello = new Label($"{_role}")
            {
                X = Pos.AnchorEnd(_role.Length) - 1,
                Y = Pos.AnchorEnd(1),
                ColorScheme = Colors.Menu
            };
            win.Add(hello);
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
                var delete = new Button("Delete")
                {
                    X = Pos.Right(cancel) + 2,
                    Y = Pos.Top(cancel)
                };
                delete.Clicked += () =>
                {
                    if (GuiHelper.Delete())
                    {
                        _superCreator.DeleteQuiz(themesList.SelectedItem);
                    }
                    top.Running = false;
                };
                win.Add(header, themesList, delete, cancel);
            }
            Application.Run();
            return (keepOn, logout, back, action!);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) CreateAdminWindow()
        {
            bool logout;
            bool keepOn;
            bool back;
            SomeAction? action = null;
            do
            {
                var stop = CreateAdminMenu();
                logout = stop.logout;
                keepOn = stop.keep_on;
                back = stop.back;
            } while (!logout && keepOn && !back);
            return (keepOn, logout, back, action!);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) CreateAdminMenu()
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
            var hello = new Label($"{_role}")
            {
                X = Pos.AnchorEnd(_role.Length) - 1,
                Y = Pos.AnchorEnd(1),
                ColorScheme = Colors.Menu
            };
            win.Add(hello);
            var header = new Label("New admin registration: ")
            {
                X = Pos.Center(),
                Y = 2
            };
            var firstName = new Label("First Name: ")
            {
                X = Pos.Center() - 19,
                Y = Pos.Top(header) + 4
            };
            var lastName = new Label("Last Name: ")
            {
                X = Pos.Left(firstName),
                Y = Pos.Top(firstName) + 2
            };
            var login = new Label("Login: ")
            {
                X = Pos.Left(firstName),
                Y = Pos.Top(lastName) + 2
            };
            var password = new Label("Password: ")
            {
                X = Pos.Left(firstName),
                Y = Pos.Top(login) + 2
            };
            var password2 = new Label("Confirm password: ")
            {
                X = Pos.Left(firstName),
                Y = Pos.Top(password) + 2
            };
            var dateOfBirth = new Label("Date of birth: ")
            {
                X = Pos.Left(firstName),
                Y = Pos.Top(password2) + 2
            };
            win.Add(header, firstName, lastName, login, password, password2, dateOfBirth);
            var firstNameText = new TextField("")
            {
                X = Pos.Right(password2),
                Y = Pos.Top(firstName),
                Width = 20
            };
            var lastNameText = new TextField("")
            {
                X = Pos.Right(password2),
                Y = Pos.Top(lastName),
                Width = 20
            };
            var loginText = new TextField("")
            {
                X = Pos.Right(password2),
                Y = Pos.Top(login),
                Width = 20
            };
            var passText = new TextField("")
            {
                Secret = true,
                X = Pos.Left(loginText),
                Y = Pos.Top(password),
                Width = Dim.Width(loginText)
            };
            var passText2 = new TextField("")
            {
                Secret = true,
                X = Pos.Left(loginText),
                Y = Pos.Top(password2),
                Width = Dim.Width(loginText)
            };
            var dateText = new TextField("dd.mm.yyyy")
            {
                X = Pos.Left(loginText),
                Y = Pos.Top(dateOfBirth),
                Width = Dim.Width(loginText)
            };
            win.Add(firstNameText, lastNameText, loginText, passText, passText2, dateText);
            var whichAdmin = new RadioGroup(new ustring[] { "Admin", "SuperAdmin" })
            {
                X = Pos.Center(),
                Y = Pos.Top(dateText) + 2,
                DisplayMode = DisplayModeLayout.Horizontal
            };
            var register = new Button("Done")
            {
                X = Pos.Center() + 1,
                Y = Pos.Top(whichAdmin) + 3
            };
            var returnBack = new Button("Cancel")
            {
                X = Pos.Left(register) - 11,
                Y = Pos.Top(register)
            };
            returnBack.Clicked += () =>
            {
                back = true; top.Running = false;
            };
            User? newuser = null;
            register.Clicked += () =>
            {
                if (firstNameText.Text == "" || lastNameText.Text == "" || loginText.Text == "" || passText.Text == "" || passText2.Text == "" || dateText.Text == "")
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "Not all fields are filled in!", "Ok");
                }
                else if (char.IsDigit(loginText.Text.ToString()![0]))
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "Login cannot start with a digit!", "Ok");
                    loginText.Text = "";
                }
                else if (loginText.Text.ToString()!.Contains(' '))
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "Login cannot contain spaces!", "Ok");
                    loginText.Text = "";
                }
                else if (passText.Text != passText2.Text)
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "Passwords don't match!", "Ok");
                    passText.Text = "";
                    passText2.Text = "";
                }
                else
                {
                    Authentification auth = new();
                    var reply = auth.RegisterAdmin(firstNameText.Text.ToString()!, lastNameText.Text.ToString()!, dateText.Text.ToString()!, loginText.Text.ToString()!, passText.Text.ToString()!, whichAdmin.SelectedItem);
                    if (reply == (false, null))
                    {
                        MessageBox.ErrorQuery(30, 7, "Error!", "This login is already occupied!", "Ok");
                        loginText.Text = "";
                    }
                    else
                    {
                        newuser = reply.user;
                        MessageBox.Query(30, 7, "Well done!", "Registration was successful!", "Ok");
                        back = true;
                        top.Running = false;
                    }
                }
            };
            win.Add(register, returnBack, whichAdmin);
            Application.Run();
            return (keepOn, logout, back, action!);
        }
        public override (bool keep_on, bool logout) AdminMenu()
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
