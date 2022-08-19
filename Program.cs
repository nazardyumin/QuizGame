using QuizGame.GUI;
using QuizGame.Users;

bool keep_on_using;
bool logout;

do
{
    (bool keep_on, User user) = GuiCommon.StartMenu();
    keep_on_using = keep_on;
    if (!keep_on_using) break;
    do
    {
        if (user.IsAdmin)
        {
            var GUIadmin = new GuiAdmin(user);
            (bool keep_on, bool logout) reply = GUIadmin.AdminMenu();
            keep_on_using = reply.keep_on;
            logout = reply.logout;
            if (!keep_on_using) break;
        }
        else if (user.IsSuperAdmin)
        {
            var GUIsuperadmin = new GuiSuperAdmin(user);
            (bool keep_on, bool logout) reply = GUIsuperadmin.AdminMenu();
            keep_on_using = reply.keep_on;
            logout = reply.logout;
            if (!keep_on_using) break;
        }
        else
        {
            var GUIplayer = new GuiPlayer(user);
            (bool keep_on, bool logout) reply = GUIplayer.PlayerMenu();
            keep_on_using = reply.keep_on;
            logout = reply.logout;
            if (!keep_on_using) break;
        }
    } while (!logout);
} while (keep_on_using);
