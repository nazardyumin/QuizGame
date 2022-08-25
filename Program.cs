using QuizGame.GUI;
using QuizGame.Users;

bool keepOnUsing;

do
{
    (bool keepOn, User user) = GuiCommon.StartMenu();
    keepOnUsing = keepOn;
    if (!keepOnUsing) break;
    bool logout;
    do
    {
        if (user.IsAdmin)
        {
            var guiAdmin = new GuiAdmin(user);
            (bool keep_on, bool logout) reply = guiAdmin.AdminMenu();
            keepOnUsing = reply.keep_on;
            logout = reply.logout;
            if (!keepOnUsing) break;
        }
        else if (user.IsSuperAdmin)
        {
            var guiSuperAdmin = new GuiSuperAdmin(user);
            (bool keep_on, bool logout) reply = guiSuperAdmin.AdminMenu();
            keepOnUsing = reply.keep_on;
            logout = reply.logout;
            if (!keepOnUsing) break;
        }
        else
        {
            var guiPlayer = new GuiPlayer(user);
            (bool keep_on, bool logout) reply = guiPlayer.PlayerMenu();
            keepOnUsing = reply.keep_on;
            logout = reply.logout;
            if (!keepOnUsing) break;
        }
    } while (!logout);
} while (keepOnUsing);
