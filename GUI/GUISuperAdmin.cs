using QuizGame.Helpers;
using Terminal.Gui;
using NStack;

namespace QuizGame.GUI
{
    public class GUISuperAdmin : GUIAdmin
    {
        SuperQuizCreator _superCreator;
        public GUISuperAdmin(User user) : base(user)
        {
            _superCreator = new(user);
            _changePass = _superCreator.ChangePassword;
            _changeDate = _superCreator.ChangeDateOfBirth;
        }
        private new (bool keep_on, bool logout, bool back, SomeAction action) MainMenuWindow() 
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
            var create_quiz = new Button("Create New Quiz");
            create_quiz.X = Pos.Center();
            create_quiz.Y = 5;
            create_quiz.Clicked += () =>
            {
                action = CreateQuizWindow;
                top.Running = false;
            };
            var edit_quiz = new Button("Edit Quiz");
            edit_quiz.X = Pos.Center();
            edit_quiz.Y = Pos.Top(create_quiz) + 2;
            edit_quiz.Clicked += () =>
            {
                action = EditQuizWindow;
                top.Running = false;
            };
            var delete_quiz = new Button("Delete Quiz");
            delete_quiz.X = Pos.Center();
            delete_quiz.Y = Pos.Top(edit_quiz) + 2;
            delete_quiz.Clicked += () =>
            {
                action = DeleteQuizWindow;
                top.Running = false;
            };
            var create_admin = new Button("Create New Admin");
            create_admin.X = Pos.Center();
            create_admin.Y = Pos.Top(delete_quiz) + 2;
            create_admin.Clicked += () =>
            {
                action = CreateAdminWindow;
                top.Running = false;
            };
            win.Add(create_quiz, edit_quiz,delete_quiz, create_admin);
            Application.Run();
            return (keep_on, logout, back, action);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) DeleteQuizWindow()
        {
            bool logout;
            bool keep_on;
            bool back;
            SomeAction action = null;
            (bool keep_on, bool logout, bool back, SomeAction action) stop;
            do
            {
                stop = DeleteQuizMenu();
                logout = stop.logout;
                keep_on = stop.keep_on;
                back = stop.back;
            } while (logout == false && keep_on == true && back == false);
            return (keep_on, logout, back, action);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) DeleteQuizMenu()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction action = null;
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
             new MenuItem("_Quit", "", () => {  if (GUIHelper.Quit()) { keep_on = false; top.Running = false;} }) })});
            top.Add(menu);
            var hello = new Label($"{_role}")
            {
                X = Pos.AnchorEnd(_role.Length) - 1,
                Y = Pos.AnchorEnd(1)
            };
            hello.ColorScheme = Colors.Menu;
            win.Add(hello);
            var header = new Label("Available Quizes: ")
            {
                X = Pos.Center(),
                Y = 2
            };
            if (_creator.GetAllQuizThemes().Count()==0)
            {
                var no_quizes = new Label("No Quizes available!")
                {
                    X = Pos.Center(),
                    Y = Pos.Bottom(header) + 4
                };
                var return_back = new Button("Back");
                return_back.X = Pos.Center();
                return_back.Y = Pos.Bottom(no_quizes) + 3;
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
                var themes_list = new RadioGroup(buffer);
                themes_list.X = Pos.Center();
                themes_list.Y = Pos.Top(header) + 3;
                var cancel = new Button("Cancel");
                cancel.X = Pos.Center() - 11;
                cancel.Y = Pos.Bottom(themes_list) + 3;
                cancel.Clicked += () =>
                {
                    back = true;
                    top.Running = false;
                };
                var delete = new Button("Delete");
                delete.X = Pos.Right(cancel) + 2;
                delete.Y = Pos.Top(cancel);
                delete.Clicked += () =>
                {
                    if (GUIHelper.Delete())
                    {
                        _superCreator.DeleteQuiz(themes_list.SelectedItem);
                    }
                    top.Running = false;
                };
                win.Add(header, themes_list, delete, cancel);
            }           
            Application.Run();
            return (keep_on, logout, back, action);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) CreateAdminWindow()
        {
            bool logout;
            bool keep_on;
            bool back;
            SomeAction action = null;
            (bool keep_on, bool logout, bool back, SomeAction action) stop;
            do
            {
                stop = CreateAdminMenu();
                logout = stop.logout;
                keep_on = stop.keep_on;
                back = stop.back;
            } while (logout == false && keep_on == true && back == false);
            return (keep_on, logout, back, action);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) CreateAdminMenu()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction action = null;
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
             new MenuItem("_Quit", "", () => {  if (GUIHelper.Quit()) { keep_on = false; top.Running = false;} }) })});
            top.Add(menu);
            var hello = new Label($"{_role}")
            {
                X = Pos.AnchorEnd(_role.Length) - 1,
                Y = Pos.AnchorEnd(1)
            };
            hello.ColorScheme = Colors.Menu;
            win.Add(hello);
            var header = new Label("New admin registration: ")
            {
                X = Pos.Center(),
                Y = 2
            };
            var first_name = new Label("First Name: ")
            {
                X = Pos.Center() - 19,
                Y = Pos.Top(header) + 4
            };
            var last_name = new Label("Last Name: ")
            {
                X = Pos.Left(first_name),
                Y = Pos.Top(first_name) + 2
            };
            var login = new Label("Login: ")
            {
                X = Pos.Left(first_name),
                Y = Pos.Top(last_name) + 2
            };
            var password = new Label("Password: ")
            {
                X = Pos.Left(first_name),
                Y = Pos.Top(login) + 2
            };
            var password2 = new Label("Confirm password: ")
            {
                X = Pos.Left(first_name),
                Y = Pos.Top(password) + 2
            };
            var date_of_birth = new Label("Date of birth: ")
            {
                X = Pos.Left(first_name),
                Y = Pos.Top(password2) + 2
            };
            win.Add(header, first_name, last_name, login, password, password2, date_of_birth);
            var first_nameText = new TextField("")
            {
                X = Pos.Right(password2),
                Y = Pos.Top(first_name),
                Width = 20
            };
            var last_nameText = new TextField("")
            {
                X = Pos.Right(password2),
                Y = Pos.Top(last_name),
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
                Y = Pos.Top(date_of_birth),
                Width = Dim.Width(loginText)
            };
            win.Add(first_nameText, last_nameText, loginText, passText, passText2, dateText);
            var which_admin = new RadioGroup(new ustring[] { "Admin", "SuperAdmin" }, 0)
            {
                X = Pos.Center(),
                Y = Pos.Top(dateText) + 2,
            };
            which_admin.DisplayMode=DisplayModeLayout.Horizontal;
            var register = new Button("Done");
            register.X = Pos.Center() + 1;
            register.Y = Pos.Top(which_admin) + 3;
            var return_back = new Button("Cancel");
            return_back.X = Pos.Left(register) - 11;
            return_back.Y = Pos.Top(register);
            return_back.Clicked += () =>
            {
                back = true; top.Running = false;
            };
            User newuser = null;
            register.Clicked += () =>
            {
                if (first_nameText.Text == "" || last_nameText.Text == "" || loginText.Text == "" || passText.Text == "" || passText2.Text == "" || dateText.Text == "")
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "Not all fields are filled in!", "Ok");
                }
                else if (char.IsDigit(loginText.Text.ToString()[0]))
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "Login cannot start with a digit!", "Ok");
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
                    Authentification Auth = new();
                    var reply = Auth.RegisterAdmin(first_nameText.Text.ToString(), last_nameText.Text.ToString(), dateText.Text.ToString(), loginText.Text.ToString(), passText.Text.ToString(),which_admin.SelectedItem);
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
            win.Add(register, return_back, which_admin);
            Application.Run();
            return (keep_on, logout, back, action);
        }
        public override (bool keep_on, bool logout) AdminMenu()
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
