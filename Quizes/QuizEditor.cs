using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class QuizEditor
{
    protected Quiz _quiz { get; set; }
    public QuizEditor()
    {
        _quiz = new Quiz();
        _quiz.Questions = new List<QuizQuestion>();
    }
    public void SetTheme(string theme)
    {
        _quiz.Theme = theme;
    }
    public void SetLevel(string level)
    {
        _quiz.Level = level;
    }
    public void EditQuestion(string question, int index)
    {
        _quiz.Questions[index].Question = question;
    }
    public void EditAnswer(string answer, int iscorrect, int index1, int index2)
    {
        _quiz.Questions[index1].Answers[index2].Answer = answer;
        _quiz.Questions[index1].Answers[index2].IsCorrect = iscorrect;
    }
    public int GetCount()
    {
        return _quiz.Questions.Count();
    }
    public string GetQuestion(int index)
    {
        return _quiz.Questions[index].Question;
    }
    public (string, int) GetAnswer(int index1, int index2)
    {
        return (_quiz.Questions[index1].Answers[index2].Answer, _quiz.Questions[index1].Answers[index2].IsCorrect);
    }
    public void QuizInit(Quiz quiz)
    {
        _quiz = quiz;
    }
}


