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
        Task<QuestionsAnswers> AddAnswerToQuestion(QuestionsAnswers questionsAnswers);

        // UPDATE (Async)
        Task<Question> Update(Question question);

        // DELETE (Async)
        Task<Question> Delete(Question question);
        Task<QuizzesQuestions> DeleteQuestionFromQuiz(QuizzesQuestions quizzesQuestions);

        // READ 
        Task<IEnumerable<Question>> GetAllQuestionsAsync();
        Task<Question> GetQuestionByIdAsync(Guid questionId);
        Task<IEnumerable<Question>> GetAllQuestionsByQuestionAsync(string question);
        Task<IEnumerable<QuestionsAnswers>> GetAllQuestionsByAnswerAsync(Guid answerId);
        Task<IEnumerable<Question>> GetAllQuestionsByQuizAsync(Guid questionId);
    }
}
