﻿using Terminal.Gui;

namespace QuizGame.Helpers
{
    public static class GUIHelper
    {
        public static bool Quit()
        {
            var n = MessageBox.Query(30, 7, "Quit", "Are you shure?", "Yes", "No");
            switch (n)
            {
                case 0: return true;
                case 1: return false;
                case -1: return false;
                default: return false;
            }
        }
        public static bool Save()
        {
            var n = MessageBox.Query(30, 7, "Save", "Are you shure?", "Yes", "No");
            switch (n)
            {
                case 0: return true;
                case 1: return false;
                case -1: return false;
                default: return false;
            }
        }
        public static bool Finish()
        {
            var n = MessageBox.Query(30, 7, "Finish", "Are you shure?", "Yes", "No");
            switch (n)
            {
                case 0: return true;
                case 1: return false;
                case -1: return false;
                default: return false;
            }
        }
        public static bool ForcedFinish()
        {
            var n = MessageBox.Query(30, 7, "Finish", "Are you shure that you've answered\nall the questions?", "Yes", "No");
            switch (n)
            {
                case 0: return true;
                case 1: return false;
                case -1: return false;
                default: return false;
            }
        }
    }
}