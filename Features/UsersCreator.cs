﻿public class UsersCreator : QuizCreator
{
    public UsersCreator(User user):base(user)
    {
    }
    public void DeleteQuiz(int index)
    {
        var list_quizes = QuizLoader.FromFile();
        list_quizes.Remove(list_quizes[index]);
        QuizSaver.RefreshQuizList(list_quizes);
    }
}