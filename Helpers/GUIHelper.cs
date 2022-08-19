using Terminal.Gui;

namespace QuizGame.Helpers
{
    public static class GuiHelper
    {
        public static bool Quit()
        {
            var n = MessageBox.Query(30, 7, "Quit", "Are you shure?", "Yes", "No");
            return n switch
            {
                0 => true,
                1 => false,
                -1 => false,
                _ => false,
            };
        }
        public static bool Save()
        {
            var n = MessageBox.Query(30, 7, "Save", "Are you shure?", "Yes", "No");
            return n switch
            {
                0 => true,
                1 => false,
                -1 => false,
                _ => false,
            };
        }
        public static bool Finish()
        {
            var n = MessageBox.Query(30, 7, "Finish", "Are you shure?", "Yes", "No");
            return n switch
            {
                0 => true,
                1 => false,
                -1 => false,
                _ => false,
            };
        }
        public static bool ForcedFinish()
        {
            var n = MessageBox.Query(30, 7, "Finish", "Are you shure that you've answered\nall the questions?", "Yes", "No");
            return n switch
            {
                0 => true,
                1 => false,
                -1 => false,
                _ => false,
            };
        }
        public static bool Cancel()
        {
            var n = MessageBox.Query(35, 10, "Cancel", "\nBy clicking \"Yes\" you will exit\nthe editing window!!!\n\nAre you shure?", "Yes", "No");
            return n switch
            {
                0 => true,
                1 => false,
                -1 => false,
                _ => false,
            };
        }
        public static bool Delete()
        {
            var n = MessageBox.Query(35, 10, "Delete", "\nBy clicking \"Yes\" you permanently\ndelete the Quiz!!!\n\nContinue?", "Yes", "No");
            return n switch
            {
                0 => true,
                1 => false,
                -1 => false,
                _ => false,
            };
        }
    }
}
