using QuizGame.Helpers;
using Terminal.Gui;

namespace QuizGame.GUI
{
    public static class GUICommon
    {
        private static (bool is_existed_user, bool comeback, bool keep_on) Welcome()
        {
            bool comeback = true;
            bool keep_on = true;
            bool is_existed_user = true;
            string quizgame = @"  
  #####                       #####                          
 #     #  #    #  #  ######  #     #    ##    #    #  ###### 
 #     #  #    #  #      #   #         #  #   ##  ##  #      
 #     #  #    #  #     #    #  ####  #    #  # ## #  #####  
 #   # #  #    #  #    #     #     #  ######  #    #  #      
 #    #   #    #  #   #      #     #  #    #  #    #  #      
  #### #   ####   #  ######   #####   #    #  #    #  ######";
            Application.Init();
            var top = Application.Top;
            var win = new Window("QuizGame")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            var menu = new MenuBar(new MenuBarItem[] { new MenuBarItem("_Menu", new MenuItem[] { new MenuItem("_Quit", "", () =>
            {
                 if (GUIHelper.Quit()){ comeback = false; keep_on = false; top.Running = false; } }) }) });
            top.Add(win, menu);
            var logo = new Label(quizgame) { X = Pos.Center(), Y = 2 };
            var signin = new Button("Enter");
            signin.X = Pos.Center();
            signin.Y = 15;
            signin.Clicked += () =>
            {
                top.Running = false;
            };
            var reg = new Button("Register");
            reg.X = Pos.Center();
            reg.Y = 17;
            reg.Clicked += () =>
            {
                is_existed_user = false;
                top.Running = false;
            };
            win.Add(logo, signin, reg);
            Application.Run();
            return (is_existed_user, comeback, keep_on);
        }
        private static (bool comeback, bool keep_on, User user) LoginPasswordWindow()
        {
            bool comeback = false;
            bool keep_on = true;
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
            {new MenuItem("_Quit", "", () => { if (GUIHelper.Quit()) {comeback=false; keep_on = false;top.Running = false; } }) })});
            top.Add(menu);
            var login = new Label("Login: ")
            {
                X = Pos.Center() - 15,
                Y = 10
            };
            var password = new Label("Password: ")
            {
                X = Pos.Left(login),
                Y = Pos.Top(login) + 2
            };
            win.Add(login, password);
            var loginText = new TextField("")
            {
                X = Pos.Right(password),
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
            win.Add(loginText, passText);
            var signin = new Button("Enter");
            signin.X = Pos.Center() + 1;
            signin.Y = Pos.Top(passText) + 3;
            User user = null;
            signin.Clicked += () =>
            {
                Authentification Auth = new();
                var reply = Auth.SignIn(loginText.Text.ToString(), passText.Text.ToString());
                if (reply == ("login", false, null))
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "Invalid Login!", "Ok");
                    loginText.Text = "";
                    passText.Text = "";
                }
                else if (reply == ("password", false, null))
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "Invalid Password!", "Ok");
                    passText.Text = "";
                }
                else
                {
                    user = reply.user;
                    top.Running = false;
                }
            };
            var back = new Button("Cancel");
            back.X = Pos.Left(signin) - 11;
            back.Y = Pos.Top(signin);
            back.Clicked += () =>
            {
                comeback = true; top.Running = false;
            };
            win.Add(signin, back);
            Application.Run();
            return (comeback, keep_on, user);
        }
        private static (bool comeback, bool keep_on, User user) RegistrationWindow()
        {
            bool comeback = false;
            bool keep_on = true;
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
            {new MenuItem("_Quit", "", () => { if (GUIHelper.Quit()) {comeback=false; keep_on = false; top.Running = false; } }) })});
            top.Add(menu);
            var header = new Label("New user registration: ")
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
            var register = new Button("Done");
            register.X = Pos.Center() + 1;
            register.Y = Pos.Top(dateText) + 3;
            var back = new Button("Cancel");
            back.X = Pos.Left(register) - 11;
            back.Y = Pos.Top(register);
            back.Clicked += () =>
            {
                comeback = true; top.Running = false;
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
                    var reply = Auth.Register(first_nameText.Text.ToString(), last_nameText.Text.ToString(), dateText.Text.ToString(), loginText.Text.ToString(), passText.Text.ToString());
                    if (reply == (false, null))
                    {
                        MessageBox.ErrorQuery(30, 7, "Error!", "This login is already occupied!", "Ok");
                        loginText.Text = "";
                    }
                    else
                    {
                        newuser = reply.user;
                        MessageBox.Query(30, 7, "Well done!", "Registration was successful!", "Ok");
                        comeback = false;
                        top.Running = false;
                    }
                }
            };
            win.Add(register, back);
            Application.Run();
            return (comeback, keep_on, newuser);
        }
        public static (bool keep_on, User user) StartMenu()
        {
            User user = null;
            bool keep_on = true;
            (bool comeback, bool keep_on, User user) reply = (false, true, null);
            do
            {
                (bool is_existed_user, bool comeback, bool keep_on) welcome = Welcome();
                keep_on = welcome.keep_on;
                if (!keep_on) break;
                if (welcome.is_existed_user)
                {
                    reply = LoginPasswordWindow();
                    keep_on = reply.keep_on;
                }
                else
                {
                    reply = RegistrationWindow();
                    keep_on = reply.keep_on;
                }
            } while (reply.comeback);
            return (keep_on, reply.user);
        }
    }
}