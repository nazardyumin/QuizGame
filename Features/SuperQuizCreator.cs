using QuizGame.Helpers;
using QuizGame.Quizes.QuizResults;
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
            var listQuizes = QuizLoader.FromFile();
            Top20Serializer.DeleteTop20File($"{listQuizes[index].Theme} ({listQuizes[index].Level})");
            listQuizes.Remove(listQuizes[index]);
            QuizSaver.RefreshQuizList(listQuizes);
        }
    }
}
