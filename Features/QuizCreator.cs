using QuizGame.Helpers;
using QuizGame.Quizes;
using QuizGame.Quizes.QuizParts;
using QuizGame.Users;

namespace QuizGame.Features
{
    public class QuizCreator : QuizEditor
    {
        private string? _question;
        private readonly List<QuizAnswer> _answers;
        public QuizCreator(User user) : base(user)
        {
            _quiz = new Quiz
            {
                Questions = new List<QuizQuestion>()
            };
            _answers = new List<QuizAnswer>();
        }
        public void SetQuestion(string question)
        {
            _question = question;
        }
        public void SetAnswer(string answer, int isCorrect)
        {
            _answers.Add(new QuizAnswer { Answer = answer, IsCorrect = isCorrect });
        }
        public void AddItem()
        {
            var temp = new QuizQuestion
            {
                Answers = new List<QuizAnswer>()
            };
            foreach (var item in _answers)
            {
                temp.Answers.Add(item);
            }
            temp.Question = _question!;
            _quiz!.Questions!.Add(temp);
            _question = "";
            _answers.Clear();
        }
        public void Clear()
        {
            _quiz!.Questions!.Clear();
        }
        public Quiz FindQuiz(string theme)
        {
            return QuizLoader.FindQuiz(theme, QuizLoader.FromFile());
        }
        public Quiz GetQuiz()
        {
            return _quiz!;
        }
    }
}
