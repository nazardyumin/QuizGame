using QuizGame.GUI;

bool keep_on_using = true;
bool logout = false;

do
{
    (bool keep_on, User user) reply1 = GUICommon.StartMenu();
    keep_on_using = reply1.keep_on;
    if (!keep_on_using) break;
    do
    {
        if (reply1.user.IsAdmin)
        {
            var GUIadmin = new GUIAdmin(reply1.user);
            (bool keep_on, bool logout) reply2 = GUIadmin.AdminMenu();
            keep_on_using = reply2.keep_on;
            logout = reply2.logout;
            if (keep_on_using == false) break;
        }
        else if (reply1.user.IsSuperAdmin)
        {
            var GUIsuperadmin = new GUISuperAdmin(reply1.user);
            (bool keep_on, bool logout) reply2 = GUIsuperadmin.SuperAdminMenu();
            keep_on_using = reply2.keep_on;
            logout = reply2.logout;
            if (keep_on_using == false) break;
        }
        else
        {
            var GUIplayer = new GUIPlayer(reply1.user);
            (bool keep_on, bool logout) reply2 = GUIplayer.PlayerMenu();
            keep_on_using = reply2.keep_on;
            logout = reply2.logout;
            if (keep_on_using == false) break;
        }
    } while (logout == false);
} while (keep_on_using);

//DON'T LOOK HERE!

Console.WriteLine("Мы здесь!");

//GUICommon.LoginPasswordWindow();


//// Creates a menubar, the item "New" has a help menu.
//var menu = new MenuBar(new MenuBarItem[] {
//			new MenuBarItem ("_Menu", new MenuItem [] {
//				new MenuItem ("_Quit", "", () => { if (Quit ()) authentification.Running = false; })
//			})
//		});
//authentification.Add(menu);





//var login = new Label("Login: ") { X = 3, Y = 2 };
//var password = new Label("Password: ")
//{
//	X = Pos.Left(login),
//	Y = Pos.Top(login) + 1
//};
//var loginText = new TextField("")
//{
//	X = Pos.Right(password),
//	Y = Pos.Top(login),
//	Width = 40
//};
//var passText = new TextField("")
//{
//	Secret = true,
//	X = Pos.Left(loginText),
//	Y = Pos.Top(password),
//	Width = Dim.Width(loginText)
//};

//// Add some controls, 
//win.Add(
//	// The ones with my favorite layout system, Computed
//	login, password, loginText, passText,

//	// The ones laid out like an australopithecus, with Absolute positions:
//	new CheckBox(3, 6, "Remember me"),
//	new RadioGroup(3, 8, new ustring[] { "_Personal", "_Company" }, 0),

//	new Button(10, 14, "Cancel"),
//	new Label(3, 18, "Press F9 or ESC plus 9 to activate the menubar")
//);
//var ok = new Button(3, 14, "Ok");
//ok.Clicked += ()=> { MessageBox.ErrorQuery(50, 7, "Error", "Login is invalid!", "Ok"); };

//win.Add(ok);
//Application.Run();

//var n = MessageBox.Query(50, 7, "Error", "Login is invalid!", "Ok");


















//Application.Shutdown();






//var test=new CreateQuiz();

/*test.SetTheme("porn");
test.SetQuestion("how deep is your pussy?");
test.SetAnswer("10", false);
test.SetAnswer("11", false);
test.SetAnswer("15", false);
test.SetAnswer("25", true);
test.AddItem();
test.SetQuestion("how deep is your pussy?");
test.SetAnswer("10", false);
test.SetAnswer("11", false);
test.SetAnswer("15", false);
test.SetAnswer("25", true);
test.AddItem();
test.SetQuestion("how deep is your pussy?");
test.SetAnswer("10", false);
test.SetAnswer("11", false);
test.SetAnswer("15", false);
test.SetAnswer("25", true);
test.AddItem();
test.SaveToQuizListFile();*/
/*Authentification start = new();
(string, bool, User) res = start.SignIn("player", "player");
var play = new PlayQuiz(res.Item3);
foreach (var item in play.GetAllQuizThemes())
{
    Console.WriteLine(item);
}
play.FindQuizAndInit("sex");
*/

/*Authentification start = new ();

//Console.WriteLine(start.Register("Nazar", "Nazar", "dateofbirth", "nazar", "nazar"));
(string,bool,User) res = start.SignIn("player", "player");
(string, bool, User) res2 = start.SignIn("player", "player");
start.SaveAndExit();

var quiz = new PlayQuiz(res.Item3);
quiz.SaveResults();

*/



//Application.Init();


//var authentification = Application.Top;

//// Creates the top-level window to show
//var win = new Window("QuizGame")
//{
//	X = 0,
//	Y = 1, // Leave one row for the toplevel menu

//	// By using Dim.Fill(), it will automatically resize without manual intervention
//	Width = Dim.Fill(),
//	Height = Dim.Fill()
//};
//authentification.Add(win);

//// Creates a menubar, the item "New" has a help menu.
//var menu = new MenuBar(new MenuBarItem[] {
//			new MenuBarItem ("_Menu", new MenuItem [] {
//				new MenuItem ("_Quit", "", () => { if (Quit ()) authentification.Running = false; })
//			})
//		});
//authentification.Add(menu);

//static bool Quit()
//{
//	var n = MessageBox.Query(50, 7, "Quit Demo", "Are you sure you want to quit this demo?", "Yes", "No");
//	return n == 0;
//}



//var login = new Label("Login: ") { X = 3, Y = 2 };
//var password = new Label("Password: ")
//{
//	X = Pos.Left(login),
//	Y = Pos.Top(login) + 1
//};
//var loginText = new TextField("")
//{
//	X = Pos.Right(password),
//	Y = Pos.Top(login),
//	Width = 40
//};
//var passText = new TextField("")
//{
//	Secret = true,
//	X = Pos.Left(loginText),
//	Y = Pos.Top(password),
//	Width = Dim.Width(loginText)
//};

//// Add some controls, 
//win.Add(
//	// The ones with my favorite layout system, Computed
//	login, password, loginText, passText,

//	// The ones laid out like an australopithecus, with Absolute positions:
//	new CheckBox(3, 6, "Remember me"),
//	new RadioGroup(3, 8, new ustring[] { "_Personal", "_Company" }, 0),

//	new Button(10, 14, "Cancel"),
//	new Label(3, 18, "Press F9 or ESC plus 9 to activate the menubar")
//);
//var ok = new Button(3, 14, "Ok");
//ok.Clicked += ()=> { MessageBox.ErrorQuery(50, 7, "Error", "Login is invalid!", "Ok"); };

//win.Add(ok);
//Application.Run();

//var n = MessageBox.Query(50, 7, "Error", "Login is invalid!", "Ok");


















//Application.Shutdown();






