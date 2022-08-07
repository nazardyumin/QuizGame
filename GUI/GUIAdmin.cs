using QuizGame.Helpers;
using Terminal.Gui;

namespace QuizGame.GUI
{
    public class GUIAdmin
    {

        private AdminFeatures? _adminFeatures;
        private delegate (bool keep_on, bool logout, bool back, SomeAction action) SomeAction();
        public GUIAdmin(User user)
        {
            _adminFeatures = new AdminFeatures(user);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) MainMenuWindow()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction action = null;
            Application.Init();
            string role = $"admin : {_adminFeatures._user.FirstName} {_adminFeatures._user.LastName}";
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
            var hello = new Label(role)
            {
                X = Pos.AnchorEnd(role.Length) - 1,
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
            win.Add(create_quiz, edit_quiz);
            Application.Run();
            return (keep_on, logout, back, action);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) SettingsWindow()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction action = HelpFunction;
            (bool keep_on, bool logout, bool back, SomeAction action) stop;
            do
            {
                stop = SettingsMenu();
                logout = stop.logout;
                keep_on = stop.keep_on;
                action = stop.action;
                if (stop.action is not null)
                {
                    do
                    {
                        stop = stop.action.Invoke();
                        logout = stop.logout;
                        keep_on = stop.keep_on;
                        back = stop.back;
                    } while (back == false && logout == false);
                }
            } while (action is not null && logout == false);
            return (keep_on, logout, back, action);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) SettingsMenu()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction action = null;
            Application.Init();
            string role = $"admin : {_adminFeatures._user.FirstName} {_adminFeatures._user.LastName}";
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
             new MenuItem("_Quit", "", () => {  if (GUIHelper.Quit()) {keep_on = false;top.Running = false; } }) })});
            top.Add(menu);
            var hello = new Label(role)
            {
                X = Pos.AnchorEnd(role.Length) - 1,
                Y = Pos.AnchorEnd(1)
            };
            hello.ColorScheme = Colors.Menu;
            win.Add(hello);
            var header = new Label("Settings: ")
            {
                X = Pos.Center(),
                Y = 2
            };
            var changedate = new Button("Change Date Of Birth");
            changedate.X = Pos.Center();
            changedate.Y = 5;
            changedate.Clicked += () =>
            {
                action = SettingsChangeDate;
                top.Running = false;
            };
            var changepass = new Button("Change Password");
            changepass.X = Pos.Center();
            changepass.Y = Pos.Top(changedate) + 2;
            changepass.Clicked += () =>
            {
                action = SettingsChangePass;
                top.Running = false;
            };
            var comeback = new Button("Back");
            comeback.X = Pos.Center();
            comeback.Y = Pos.Top(changepass) + 3;
            comeback.Clicked += () =>
            {
                back = true; top.Running = false;
            };
            win.Add(header, changedate, changepass, comeback);
            Application.Run();
            return (keep_on, logout, back, action);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) SettingsChangeDate()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction action = null;
            Application.Init();
            string role = $"admin : {_adminFeatures._user.FirstName} {_adminFeatures._user.LastName}";
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
            var hello = new Label(role)
            {
                X = Pos.AnchorEnd(role.Length) - 1,
                Y = Pos.AnchorEnd(1)
            };
            hello.ColorScheme = Colors.Menu;
            win.Add(hello);
            var header = new Label("Changing Date Of Birth: ")
            {
                X = Pos.Center(),
                Y = 2
            };
            var old_date = new Label("Actual Date: ")
            {
                X = Pos.Center() - 13,
                Y = Pos.Top(header) + 4
            };
            var old_date_text = new Label($" {_adminFeatures._user.DateOfBirth} ")
            {
                X = Pos.Left(old_date) + 14,
                Y = Pos.Top(header) + 4
            };
            old_date_text.ColorScheme = Colors.Menu;
            var new_date = new Label("New Date: ")
            {
                X = Pos.Left(old_date) + 3,
                Y = Pos.Top(header) + 6
            };
            var new_date_text = new TextField(" ")
            {
                X = Pos.Left(old_date_text),
                Y = Pos.Top(old_date_text) + 2,
                Width = 12
            };
            var done = new Button("Done");
            done.X = Pos.Center() + 1;
            done.Y = Pos.Top(new_date_text) + 3;
            done.Clicked += () =>
            {
                var newdate = new_date_text.Text.ToString();
                if (newdate.StartsWith(" ")) newdate = newdate.Substring(1);
                if (newdate.Length == 0)
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "This field must be filled in!", "Ok");
                    new_date_text.Text = " ";
                }
                else if (newdate == _adminFeatures._user.DateOfBirth)
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "The dates match!", "Ok");
                    new_date_text.Text = " ";
                }
                else if (newdate.Length != 10)
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "Date format is incorrect!", "Ok");
                    new_date_text.Text = " ";
                }
                else
                {
                    _adminFeatures.ChangeDateOfBirth(newdate);
                    MessageBox.Query(30, 7, "Well done!", "Сhanges saved successfully!", "Ok");
                    back = true; top.Running = false;
                }
            };
            var comeback = new Button("Cancel");
            comeback.X = Pos.Left(done) - 11;
            comeback.Y = Pos.Top(new_date_text) + 3;
            comeback.Clicked += () =>
            {
                back = true; top.Running = false;
            };
            win.Add(header, old_date, old_date_text, new_date, new_date_text, done, comeback);
            Application.Run();
            return (keep_on, logout, back, action);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) SettingsChangePass()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction action = null;
            Application.Init();
            string role = $"admin : {_adminFeatures._user.FirstName} {_adminFeatures._user.LastName}";
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
            var hello = new Label(role)
            {
                X = Pos.AnchorEnd(role.Length) - 1,
                Y = Pos.AnchorEnd(1)
            };
            hello.ColorScheme = Colors.Menu;
            win.Add(hello);
            var header = new Label("Changing Password: ")
            {
                X = Pos.Center(),
                Y = 2
            };
            var new_pass = new Label("New Password: ")
            {
                X = Pos.Center() - 14,
                Y = Pos.Top(header) + 4
            };
            var new_pass_text = new TextField("")
            {
                Secret = true,
                X = Pos.Left(new_pass) + 15,
                Y = Pos.Top(new_pass),
                Width = 12
            };
            var new_pass2 = new Label("New Password: ")
            {
                X = Pos.Left(new_pass),
                Y = Pos.Top(new_pass) + 2
            };
            var new_pass_text2 = new TextField("")
            {
                Secret = true,
                X = Pos.Left(new_pass_text),
                Y = Pos.Top(new_pass_text) + 2,
                Width = 12
            };
            var old_pass = new Label("Old Password: ")
            {
                X = Pos.Left(new_pass),
                Y = Pos.Top(new_pass_text2) + 2
            };
            var old_pass_text = new TextField("")
            {
                Secret = true,
                X = Pos.Left(new_pass_text),
                Y = Pos.Top(new_pass_text2) + 2,
                Width = 12
            };
            old_pass_text.ColorScheme = Colors.Error;
            var done = new Button("Done");
            done.X = Pos.Center() + 1;
            done.Y = Pos.Top(old_pass_text) + 3;
            var comeback = new Button("Cancel");
            comeback.X = Pos.Left(done) - 11;
            comeback.Y = Pos.Top(old_pass_text) + 3;
            comeback.Clicked += () =>
            {
                back = true; top.Running = false;
            };
            done.Clicked += () =>
            {
                if (new_pass_text.Text == "" || new_pass_text2.Text == "" || old_pass_text.Text == "")
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "Not all fields are filled in!", "Ok");

                }
                else if (new_pass_text.Text != new_pass_text2.Text)
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "Confirming the New Password is incorrect!", "Ok");
                    new_pass_text.Text = new_pass_text2.Text = old_pass_text.Text = "";
                }
                else
                {
                    var newpass = new_pass_text.Text.ToString();
                    if (newpass == old_pass_text.Text)
                    {
                        MessageBox.ErrorQuery(30, 7, "Error!", "The New and the Old Passwords match!", "Ok");
                        old_pass_text.Text = "";
                    }
                    else if (old_pass_text.Text != _adminFeatures._user.Password)
                    {
                        MessageBox.ErrorQuery(30, 7, "Error!", "The Old Password is invalid!", "Ok");
                        old_pass_text.Text = "";
                    }
                    else
                    {
                        _adminFeatures.ChangePassword(newpass);
                        MessageBox.Query(30, 7, "Well done!", "Your Password change was successful!\nPlease Enter with the New Password!", "Ok");
                        logout = true; top.Running = false;
                    }
                }
            };
            win.Add(header, new_pass, new_pass_text, done, new_pass2, new_pass_text2, old_pass, old_pass_text, comeback);
            Application.Run();
            return (keep_on, logout, back, action);
        }

        //окна для создания квизов
        private (bool keep_on, bool logout, bool back, SomeAction action) CreateQuizWindow()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction action = null;
            return (keep_on, logout, back, action);
        }

        private (bool keep_on, bool logout, bool back, SomeAction action) CreateQuizMenu()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction action = null;
            return (keep_on, logout, back, action);
        }
        //окна для редактрования квизов
        private (bool keep_on, bool logout, bool back, SomeAction action) EditQuizWindow()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction action = null;
            return (keep_on, logout, back, action);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) EditQuizMenu()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction action = null;
            return (keep_on, logout, back, action);
        }

        public (bool keep_on, bool logout) AdminMenu()
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
        private (bool keep_on, bool logout, bool back, SomeAction action) HelpFunction()
        {
            return (true, false, false, MainMenuWindow);
        }
    }
}

