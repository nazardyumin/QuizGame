using QuizGame.Quizes.QuizParts;

namespace QuizGame.Quizes
{
    public class Quiz
    {
        public string? Theme { get; set; }
        public string? Level { get; set; }
        public List<QuizQuestion>? Questions { get; set; }      
    }
}
