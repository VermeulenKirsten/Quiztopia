using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quiztopia.Models.Repositories
{
    public interface IQuestionRepo
    {
        // CREATE (Async) 
        Task<Question> Add(Question question);
        Task<QuizzesQuestions> AddQuestionToQuiz(QuizzesQuestions quizzesQuestions);

        // UPDATE (Async)
        Task<Question> Update(Question question);

        // DELETE (Async)
        Task<Question> Delete(Question question);
        Task<QuizzesQuestions> DeleteQuestionFromQuiz(QuizzesQuestions quizzesQuestions);

        // READ 
        Task<IEnumerable<Question>> GetAllQuestionsAsync();
        Task<IEnumerable<Question>> GetQuestionByQuestionAsync(string question);
        Task<Question> GetQuestionByIdAsync(Guid questionId);
        Task<IEnumerable<QuizzesQuestions>> GetAllQuizzesQuestionsAsync(Guid questionId);
        Task<IEnumerable<Question>> GetAllQuestionsByQuizAsync(Guid questionId);
    }
}
