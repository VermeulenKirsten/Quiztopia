using Quiztopia.Models;
using Quiztopia.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiztopia.Web.ViewModels
{
    public class QuizQuestionVM
    {
        public Guid QuizId { get; set; }
        public Question Question { get; set; }
        public List<Question> Questions { get; set; }
        public List<Answer> AnswersFromQuestion { get; set; }

        public QuizQuestionVM()
        {

        }

        public QuizQuestionVM(Guid quizId)
        {
            this.QuizId = quizId;
        }

        public QuizQuestionVM(Guid quizId, IQuestionRepo questionRepo)
        {
            this.QuizId = quizId;
            this.Questions = new List<Question>(questionRepo.GetAllQuestionsByQuizAsync(quizId).Result);
        }

        public QuizQuestionVM(Guid quizId, IAnswerRepo answerRepo)
        {
            this.QuizId = quizId;
            this.AnswersFromQuestion= new List<Answer>(answerRepo.GetAllAnswersByQuestionAsync(Guid.Empty).Result);
        }
    }
}
