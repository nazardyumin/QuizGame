using System.Text;
using QuizGame.Helpers;
using QuizGame.Quizes;
using QuizGame.Quizes.QuizResults;
using QuizGame.Users;

namespace QuizGame.Features
{
    public class QuizPlayer : Default
    {
        private Quiz? _quiz;
        private readonly QuizResult? _result;
        private RatingPosition? _position;
        public QuizPlayer(User user) : base(user)
        {
            _quiz = new Quiz();
            _result = new QuizResult
            {
                AnsweredQuestions = new(),
                Scores = 0
            };
        }
        public void SetQuiz(Quiz quiz)
        {
            _quiz = quiz;
        }
        public List<string> GetAllQuizThemes()
        {
            var quizlist = QuizLoader.FromFile();
            var listthemes = new List<string>();
            foreach (var item in quizlist)
            {
                listthemes.Add($"{item.Theme} ({item.Level})");
            }
            return listthemes;
        }
        public Quiz? FindQuiz(int index)
        {
            return QuizLoader.FindQuiz(index);
        }
        public Quiz MixedQuiz()
        {
            return QuizLoader.MakeMixedQuiz();
        }
        public int GetCount()
        {
            return _quiz!.Questions!.Count;
        }
        public string GetTheme()
        {
            return _quiz!.Theme!;
        }
        public string GetLevel()
        {
            return _quiz!.Level!;
        }
        public string GetQuestion(int index)
        {
            return _quiz!.Questions![index].Question!;
        }
        public List<string> GetListAnswers(int index)
        {
            List<string> list = new();
            for (var i = 0; i < 4; i++)
            {
                list.Add(GetAnswer(index, i));
            }
            return list;
        }
        private string GetAnswer(int index1, int index2)
        {
            return _quiz!.Questions![index1].Answers![index2].Answer!;
        }
        public bool CheckingAnswer(int index, bool res1, bool res2, bool res3, bool res4)
        {
            var check1 = false;
            var check2 = false;
            var check3 = false;
            var check4 = false;
            if (_quiz!.Questions![index].Answers![0].IsCorrect == 1 && res1) check1 = true;
            if (_quiz!.Questions![index].Answers![0].IsCorrect == 0 && !res1) check1 = true;
            if (_quiz!.Questions![index].Answers![1].IsCorrect == 1 && res2) check2 = true;
            if (_quiz!.Questions![index].Answers![1].IsCorrect == 0 && !res2) check2 = true;
            if (_quiz!.Questions![index].Answers![2].IsCorrect == 1 && res3) check3 = true;
            if (_quiz!.Questions![index].Answers![2].IsCorrect == 0 && !res3) check3 = true;
            if (_quiz!.Questions![index].Answers![3].IsCorrect == 1 && res4) check4 = true;
            if (_quiz!.Questions![index].Answers![3].IsCorrect == 0 && !res4) check4 = true;
            if (check1 && check2 && check3 && check4) return true;
            else return false;
        }
        public void AddItemToQuizResult(int index, bool is_correct)
        {
            var item = new QuizQuestionResult
            {
                Question = _quiz!.Questions![index].Question,
                IsCorrect = is_correct
            };
            _result!.AnsweredQuestions!.Add(item);
            if (is_correct) _result.Scores++;
        }
        private void AddQuizResultToUser()
        {
            _result!.Theme = _quiz!.Theme;
            _result.Level = _quiz.Level;
            _result.Date = $"{DateTime.Now:g}";
            var database = new UsersDataBase();
            database.LoadFromFile();
            var user = database.SearchByLogin(_user.Login!);
            if (user!.Results is not null && _user!.Results is not null)
            {
                user.Results.Add(_result);
                _user.Results.Add(_result);
            }
            else
            {
                user.Results = new()
                {
                    _result
                };
                _user.Results = new()
                {
                    _result
                };
            }
            database.SaveToFile();
        }
        private void PositionInit()
        {
            _position = new RatingPosition
            {
                Login = _user!.Login,
                Name = _user.FirstName + " " + _user.LastName,
                Scores = _result!.Scores
            };
        }
        private void SaveResultToTop20()
        {
            if (_position!.Scores > 0)
            {
                if (_quiz!.Top20 is not null)
                {
                    if (_quiz.Top20.Find((i) => i.Login == _position!.Login) is not null)
                    {
                        var item = _quiz.Top20.Find((i) => i.Name == _position.Name);
                        if (item!.Scores < _position.Scores) item.Scores = _position.Scores;
                    }
                    else
                    {
                        _quiz.Top20.Add(_position);
                        var ordered = _quiz.Top20.OrderByDescending((p) => p.Scores);
                        var i = 0;
                        foreach (var item in ordered)
                        {
                            _quiz.Top20[i] = item;
                            i++;
                        }
                    }
                }
                else
                {
                    _quiz.Top20 = new List<RatingPosition>
                    {
                        _position
                    };
                }
            }
        }
        private void SaveResultToHighscoresFile()
        {
            if (_position!.Scores > 0)
            {
                SerializerHelper.SaveHighscores(_position);
            }
        }
        public void SaveResults(bool is_mixed)
        {
            AddQuizResultToUser();
            PositionInit();
            if (!is_mixed)
            {
                SaveResultToTop20();
                QuizSaver.ReSaveToFile(_quiz!);
            }
            SaveResultToHighscoresFile();
        }
        public List<string>? GetTop20()
        {
            List<string> list = new();
            if (_quiz!.Top20 is not null)
            {
                foreach (var item in _quiz.Top20)
                {
                    list.Add($"{item.Name} {item.Scores}");
                }
            }
            return list;
        }
        public string? GetHighScores()
        {
            var stringBuilder = new StringBuilder();
            var list = SerializerHelper.LoadHighscores();
            if (list is not null)
            {
                if (list.Count > 9)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        stringBuilder.Append($"{list[i]!.Name!} {list[i]!.Scores!}\n");
                    }
                }
                else
                {
                    foreach (var item in list)
                    {
                        stringBuilder.Append($"{item.Name} {item.Scores}\n");
                    }
                }
            }
            string? highscores = stringBuilder.ToString();
            return highscores;
        }
        public List<QuizResult>? GetQuizResults()
        {
            var database = new UsersDataBase();
            database.LoadFromFile();
            var user = database.SearchByLogin(_user.Login!);
            return user!.Results;
        }
        public void ResetQuizResult()
        {
            _result!.Theme = "";
            _result.Level = "";
            _result.AnsweredQuestions!.Clear();
            _result.Scores = 0;
        }
        public int GetScores()
        {
            return _result!.Scores;
        }
        public string GetPlayerInfo()
        {
            var quizes_passed = 0;
            var total_scores = 0;
            var database = new UsersDataBase();
            database.LoadFromFile();
            var user = database.SearchByLogin(_user.Login!);
            if (user!.Results is not null)
            {
                quizes_passed = user.Results.Count;
                foreach (var item in user.Results)
                {
                    total_scores += item.Scores;
                }
            }
            return $"Quizes passed: {quizes_passed} | Total Scores: {total_scores} | ";
        }
    }
}
