using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class QuizQuestionResult
{
    public string Question { get; set; }
    public List<QuizAnswerResult>? Answers { get; set; }
    public bool IsCorrect { get; set; }
}