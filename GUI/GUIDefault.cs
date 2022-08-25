using QuizGame.Helpers;
using QuizGame.Quizes.QuizResults;
using QuizGame.Users;
using Terminal.Gui;

namespace QuizGame.GUI
{
    public class GuiDefault
    {
        protected readonly User _user;
        protected readonly string _role;
        protected Change? _changePass;
        protected Change? _changeDate;
        protected delegate void Change(string @new);
        protected delegate (bool keep_on, bool logout, bool back, SomeAction action) SomeAction();
        protected GuiDefault(User user)
        {
            _user = user;
            if (user.IsAdmin)
            {
                _role = $"admin | {user.FirstName} {user.LastName}";
            }
            else if (user.IsSuperAdmin)
            {
                _role = $"superadmin | {user.FirstName} {user.LastName}";
            }
            else
            {
                _role = $"{user.FirstName} {user.LastName}";
            }
        }
        protected (bool keep_on, bool logout, bool back, SomeAction action) SettingsWindow()
        {
            bool logout;
            bool keepOn;
            bool back = false;
            SomeAction? action;
            do
            {
                var stop = SettingsMenu();
                logout = stop.logout;
                keepOn = stop.keep_on;
                action = stop.action;
                if (stop.action is not null)
                {
                    do
                    {
                        stop = stop.action.Invoke();
                        logout = stop.logout;
                        keepOn = stop.keep_on;
                        back = stop.back;
                    } while (!back && !logout && keepOn);
                }
            } while (action is not null && !logout && keepOn);
            return (keepOn, logout, back, action!);
        }
        protected (bool keep_on, bool logout, bool back, SomeAction action) SettingsMenu()
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
             new MenuItem("_Quit", "", () => {  if (GuiHelper.Quit()) {keepOn = false;top.Running = false; } }) })});
            top.Add(menu);
            var hello = new Label($"{AdditionalInfo()}{_role}")
            {
                X = Pos.AnchorEnd($"{AdditionalInfo()}{_role}".Length) - 1,
                Y = Pos.AnchorEnd(1),
                ColorScheme = Colors.Menu
            };
            win.Add(hello);
            var header = new Label("Settings: ")
            {
                X = Pos.Center(),
                Y = 2
            };
            var changeDate = new Button("Change Date Of Birth")
            {
                X = Pos.Center(),
                Y = 5
            };
            changeDate.Clicked += () =>
            {
                action = SettingsChangeDate;
                top.Running = false;
            };
            var changePass = new Button("Change Password")
            {
                X = Pos.Center(),
                Y = Pos.Top(changeDate) + 2
            };
            changePass.Clicked += () =>
            {
                action = SettingsChangePass;
                top.Running = false;
            };
            var comeback = new Button("Back")
            {
                X = Pos.Center(),
                Y = Pos.Top(changePass) + 3
            };
            comeback.Clicked += () =>
            {
                back = true; top.Running = false;
            };
            win.Add(header, changeDate, changePass, comeback);
            Application.Run();
            return (keepOn, logout, back, action!);
        }
        protected (bool keep_on, bool logout, bool back, SomeAction action) SettingsChangeDate()
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
             new MenuItem("_Quit", "", () => {  if (GuiHelper.Quit()) {keepOn = false; top.Running = false;} }) })});
            top.Add(menu);
            var hello = new Label($"{AdditionalInfo()}{_role}")
            {
                X = Pos.AnchorEnd($"{AdditionalInfo()}{_role}".Length) - 1,
                Y = Pos.AnchorEnd(1),
                ColorScheme = Colors.Menu
            };
            win.Add(hello);
            var header = new Label("Changing Date Of Birth: ")
            {
                X = Pos.Center(),
                Y = 2
            };
            var oldDate = new Label("Actual Date: ")
            {
                X = Pos.Center() - 13,
                Y = Pos.Top(header) + 4
            };
            var oldDateText = new Label($" {_user.DateOfBirth} ")
            {
                X = Pos.Left(oldDate) + 14,
                Y = Pos.Top(header) + 4,
                ColorScheme = Colors.Menu
            };
            var newDate = new Label("New Date: ")
            {
                X = Pos.Left(oldDate) + 3,
                Y = Pos.Top(header) + 6
            };
            var newDateText = new TextField(" ")
            {
                X = Pos.Left(oldDateText),
                Y = Pos.Top(oldDateText) + 2,
                Width = 12
            };
            var done = new Button("Done")
            {
                X = Pos.Center() + 1,
                Y = Pos.Top(newDateText) + 3
            };
            done.Clicked += () =>
            {
                var date = newDateText.Text.ToString();
                if (date!.StartsWith(" ")) date = date[1..];
                if (date.Length == 0)
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "This field must be filled in!", "Ok");
                    newDateText.Text = " ";
                }
                else if (date == _user.DateOfBirth)
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "The dates match!", "Ok");
                    newDateText.Text = " ";
                }
                else if (date.Length != 10)
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "Date format is incorrect!", "Ok");
                    newDateText.Text = " ";
                }
                else
                {
                    _changeDate!(date);
                    MessageBox.Query(30, 7, "Well done!", "Сhanges saved successfully!", "Ok");
                    back = true; top.Running = false;
                }
            };
            var comeback = new Button("Cancel")
            {
                X = Pos.Left(done) - 11,
                Y = Pos.Top(newDateText) + 3
            };
            comeback.Clicked += () =>
            {
                back = true; top.Running = false;
            };
            win.Add(header, oldDate, oldDateText, newDate, newDateText, done, comeback);
            Application.Run();
            return (keepOn, logout, back, action!);
        }
        protected (bool keep_on, bool logout, bool back, SomeAction action) SettingsChangePass()
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
             new MenuItem("_Quit", "", () => {  if (GuiHelper.Quit()) {keepOn = false; top.Running = false;} }) })});
            top.Add(menu);
            var hello = new Label($"{AdditionalInfo()}{_role}")
            {
                X = Pos.AnchorEnd($"{AdditionalInfo()}{_role}".Length) - 1,
                Y = Pos.AnchorEnd(1),
                ColorScheme = Colors.Menu
            };
            win.Add(hello);
            var header = new Label("Changing Password: ")
            {
                X = Pos.Center(),
                Y = 2
            };
            var newPass = new Label("New Password: ")
            {
                X = Pos.Center() - 14,
                Y = Pos.Top(header) + 4
            };
            var newPassText = new TextField("")
            {
                Secret = true,
                X = Pos.Left(newPass) + 15,
                Y = Pos.Top(newPass),
                Width = 12
            };
            var newPass2 = new Label("New Password: ")
            {
                X = Pos.Left(newPass),
                Y = Pos.Top(newPass) + 2
            };
            var newPassText2 = new TextField("")
            {
                Secret = true,
                X = Pos.Left(newPassText),
                Y = Pos.Top(newPassText) + 2,
                Width = 12
            };
            var oldPass = new Label("Old Password: ")
            {
                X = Pos.Left(newPass),
                Y = Pos.Top(newPassText2) + 2
            };
            var oldPassText = new TextField("")
            {
                Secret = true,
                X = Pos.Left(newPassText),
                Y = Pos.Top(newPassText2) + 2,
                Width = 12,
                ColorScheme = Colors.Error
            };
            var done = new Button("Done")
            {
                X = Pos.Center() + 1,
                Y = Pos.Top(oldPassText) + 3
            };
            var comeback = new Button("Cancel")
            {
                X = Pos.Left(done) - 11,
                Y = Pos.Top(oldPassText) + 3
            };
            comeback.Clicked += () =>
            {
                back = true; top.Running = false;
            };
            done.Clicked += () =>
            {
                if (newPassText.Text == "" || newPassText2.Text == "" || oldPassText.Text == "")
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "Not all fields are filled in!", "Ok");
                }
                else if (newPassText.Text != newPassText2.Text)
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "Confirming the New Password is incorrect!", "Ok");
                    newPassText.Text = newPassText2.Text = oldPassText.Text = "";
                }
                else
                {
                    var pass = newPassText.Text.ToString();
                    if (pass == oldPassText.Text)
                    {
                        MessageBox.ErrorQuery(30, 7, "Error!", "The New and the Old Passwords match!", "Ok");
                        newPassText.Text = newPassText2.Text = oldPassText.Text = "";
                    }
                    else if (oldPassText.Text != _user.Password)
                    {
                        MessageBox.ErrorQuery(30, 7, "Error!", "The Old Password is invalid!", "Ok");
                        newPassText.Text = newPassText2.Text = oldPassText.Text = "";
                    }
                    else
                    {
                        _changePass!(pass!);
                        MessageBox.Query(30, 7, "Well done!", "Your Password change was successful!\nPlease Enter with the New Password!", "Ok");
                        logout = true; top.Running = false;
                    }
                }
            };
            win.Add(header, newPass, newPassText, done, newPass2, newPassText2, oldPass, oldPassText, comeback);
            Application.Run();
            return (keepOn, logout, back, action!);
        }
        protected (bool keep_on, bool logout, bool back, SomeAction action) HelpFunction()
        {
            return (true, false, false, SettingsWindow);
        }
        private string AdditionalInfo()
        {
            if (!_user.IsAdmin && !_user.IsSuperAdmin)
            {
                var quizesPassed = 0;
                var totalScores = 0;
                var list = QuizResultsSerializer.GetQuizResults(_user.Login!);
                if (list!.Count > 0)
                {
                    quizesPassed = list.Count;
                    foreach (var item in list)
                    {
                        totalScores += item.Scores;
                    }
                }
                return $"Quizes passed: {quizesPassed} | Total Scores: {totalScores} | ";
            }
            else
            {
                return "";
            }
        }
    }
}
