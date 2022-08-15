using QuizGame.GUI;

bool keep_on_using;
bool logout;

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
            (bool keep_on, bool logout) reply2 = GUIsuperadmin.AdminMenu();
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