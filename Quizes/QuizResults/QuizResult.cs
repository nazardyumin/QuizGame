﻿public class QuizResult
{
    public string? Theme { get; set; }
    public string? Level { get; set; }
    public List<QuizQuestionResult> AnsweredQuestions { get; set; }
    public int Scores { get; set; }
}