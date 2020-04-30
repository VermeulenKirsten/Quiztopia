using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quiztopia.Models.Repositories
{
    public interface IAnswerRepo
    {
        // CREATE (Async) 
        Task<Answer> Add(Answer answer);

        // UPDATE (Async)
        Task<Answer> Update(Answer answer);

        // DELETE (Async)
        Task<Answer> Delete(Answer answer);
        Task<QuestionsAnswers> DeleteAnswerFromQuestion(QuestionsAnswers questionsAnswers);

        // READ 
        Task<IEnumerable<Answer>> GetAllAnswersAsync();
        Task<Answer> GetAnswerByIdAsync(Guid answerId);
        Task<IEnumerable<Answer>> GetAnswerByAnswerAsync(string answer, bool isCorrect);
        Task<IEnumerable<Answer>> GetAllAnswersByQuestionAsync(Guid questionId);
    }
}