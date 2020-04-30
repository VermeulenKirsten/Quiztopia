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

        // UPDATE (Async)
        Task<Question> Update(Question question);

        // DELETE (Async)
        Task<Question> Delete(Question question);

        // READ 
        Task<IEnumerable<Question>> GetAllQuestionsAsync();
        Task<Question> GetQuestionByIdAsync(int questionId);
        Task<IEnumerable<Question>> GetAllQuestionsByQuizAsync(int questionId);
    }
}
