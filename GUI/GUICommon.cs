using QuizGame.Helpers;
using QuizGame.Users;
using Terminal.Gui;

namespace QuizGame.GUI
{
    public static class GuiCommon
    {
        private static (bool is_existed_user, bool comeback, bool keep_on) Welcome()
        {
            bool comeback = true;
            bool keepOn = true;
            bool isExistedUser = true;
            string quizGame = @"  
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
                 if (GuiHelper.Quit()){ comeback = false; keepOn = false; top.Running = false; } }) }) });
            top.Add(win, menu);
            var logo = new Label(quizGame) { X = Pos.Center(), Y = 2 };
            var signin = new Button("Enter")
            {
                X = Pos.Center(),
                Y = 15
            };
            signin.Clicked += () =>
            {
                top.Running = false;
            };
            var reg = new Button("Register")
            {
                X = Pos.Center(),
                Y = 17
            };
            reg.Clicked += () =>
            {
                isExistedUser = false;
                top.Running = false;
            };
            win.Add(logo, signin, reg);
            Application.Run();
            return (isExistedUser, comeback, keepOn);
        }
        private static (bool comeback, bool keep_on, User user) LoginPasswordWindow()
        {
            bool comeback = false;
            bool keepOn = true;
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
            {new MenuItem("_Quit", "", () => { if (GuiHelper.Quit()) {comeback=false; keepOn = false;top.Running = false; } }) })});
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
            var signin = new Button("Enter")
            {
                X = Pos.Center() + 1,
                Y = Pos.Top(passText) + 3
            };
            User? user = null;
            signin.Clicked += () =>
            {
                Authentification auth = new();
                var reply = auth.SignIn(loginText.Text.ToString()!, passText.Text.ToString()!);
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
                    user = reply.user!;
                    top.Running = false;
                }
            };
            var back = new Button("Cancel")
            {
                X = Pos.Left(signin) - 11,
                Y = Pos.Top(signin)
            };
            back.Clicked += () =>
            {
                comeback = true; top.Running = false;
            };
            win.Add(signin, back);
            Application.Run();
            return (comeback, keepOn, user!);
        }
        private static (bool comeback, bool keep_on, User user) RegistrationWindow()
        {
            bool comeback = false;
            bool keepOn = true;
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
            {new MenuItem("_Quit", "", () => { if (GuiHelper.Quit()) {comeback=false; keepOn = false; top.Running = false; } }) })});
            top.Add(menu);
            var header = new Label("New user registration: ")
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
            var register = new Button("Done")
            {
                X = Pos.Center() + 1,
                Y = Pos.Top(dateText) + 3
            };
            var back = new Button("Cancel")
            {
                X = Pos.Left(register) - 11,
                Y = Pos.Top(register)
            };
            back.Clicked += () =>
            {
                comeback = true; top.Running = false;
            };
            User? newUser = null;
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
                    var reply = auth.Register(firstNameText.Text.ToString()!, lastNameText.Text.ToString()!, dateText.Text.ToString()!, loginText.Text.ToString()!, passText.Text.ToString()!);
                    if (reply == (false, null))
                    {
                        MessageBox.ErrorQuery(30, 7, "Error!", "This login is already occupied!", "Ok");
                        loginText.Text = "";
                    }
                    else
                    {
                        newUser = reply.user;
                        MessageBox.Query(30, 7, "Well done!", "Registration was successful!", "Ok");
                        comeback = false;
                        top.Running = false;
                    }
                }
            };
            win.Add(register, back);
            Application.Run();
            return (comeback, keepOn, newUser!);
        }
        public static (bool keep_on, User user) StartMenu()
        {
            bool keepOn;
            (bool comeback, bool keep_on, User? user) reply = (false, true, null);
            do
            {
                (bool is_existed_user, bool comeback, bool keep_on) welcome = Welcome();
                keepOn = welcome.keep_on;
                if (!keepOn) break;
                if (welcome.is_existed_user)
                {
                    reply = LoginPasswordWindow();
                    keepOn = reply.keep_on;
                }
                else
                {
                    reply = RegistrationWindow();
                    keepOn = reply.keep_on;
                }
            } while (reply.comeback);
            return (keepOn, reply.user!);
        }
    }
}
