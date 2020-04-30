using Quiztopia.Models;
using Quiztopia.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiztopia.Web.ViewModels
{
    public class QuestionAnswerVM
    {
        public Guid QuestionId { get; set; }
        public Answer Answer { get; set; }
        public List<Answer> Answers { get; set; }

        public QuestionAnswerVM()
        {

        }

        public QuestionAnswerVM(Guid questionId)
        {
            this.QuestionId = questionId;
        }

        public QuestionAnswerVM(Guid questionId, IAnswerRepo answerRepo)
        {
            this.QuestionId = questionId;
            this.Answers = new List<Answer>(answerRepo.GetAllAnswersByQuestionAsync(questionId).Result);
        }
    }
}
