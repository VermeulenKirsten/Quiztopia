using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quiztopia.Models.Repositories
{
    public interface IQuizRepo
    {
        Task<Quiz> Add(Quiz quiz);
        Task<QuizzesQuestions> AddQuestionToQuiz(Guid quizId, Guid questionId);
        Task<Quiz> Delete(Quiz quiz);
        Task<IEnumerable<Quiz>> GetAllQuizzesAsync();
        Task<IEnumerable<Quiz>> GetQuizzesByNameAsync(string name);
        Task<IEnumerable<QuizzesQuestions>> GetAllQuizzesByQuestionAsync(Guid questionId);
        Task<Quiz> GetQuizByIdAsync(Guid quizId);
        Task<Quiz> Update(Quiz quiz);
    }
}