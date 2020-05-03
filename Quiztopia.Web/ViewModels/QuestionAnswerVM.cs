using Quiztopia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiztopia.Web.ViewModels
{
    public class QuestionAnswerVM
    {
        // Properties
        public Guid QuizId { get; set; }
        public Question Question { get; set; }
        public Answer Answer { get; set; }
        public List<Answer> Answers { get; set; }

        public QuestionAnswerVM() { }
    }
}
