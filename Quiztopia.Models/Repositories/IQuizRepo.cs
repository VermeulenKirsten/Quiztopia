using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quiztopia.Models.Repositories
{
    public interface IQuizRepo
    {
        Task<Quiz> Add(Quiz quiz);
        Task<Quiz> Delete(Quiz quiz);
        Task<IEnumerable<Quiz>> GetAllQuizzesAsync();
        Task<IEnumerable<Quiz>> GetQuizzesByNameAsync(string name);
        Task<Quiz> GetQuizByIdAsync(int quizId);
        Task<Quiz> Update(Quiz quiz);
    }
}