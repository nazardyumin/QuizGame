using QuizGame.Helpers;
using QuizGame.Users;

namespace QuizGame.Features
{
    public class SuperQuizCreator : QuizCreator
    {
        public SuperQuizCreator(User user) : base(user)
        {
        }
        public void DeleteQuiz(int index)
        {
            var list_quizes = QuizLoader.FromFile();
            list_quizes.Remove(list_quizes[index]);
            QuizSaver.RefreshQuizList(list_quizes);
        }
    }
}
