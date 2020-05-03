using Quiztopia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiztopia.Web.ViewModels
{
    public class QuizQuestionVM
    {
        // Properties

        public Guid QuizId { get; set; }
        public Question Question { get; set; }
        public List<Question> Questions { get; set; }

        public QuizQuestionVM() { }
    }
}
