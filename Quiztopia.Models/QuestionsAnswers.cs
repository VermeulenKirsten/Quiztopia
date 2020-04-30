using System;
using System.Collections.Generic;
using System.Text;

namespace Quiztopia.Models
{
    public class QuestionsAnswers
    {
        // Properties

        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }

        // Navigation properties

        public Question Question { get; set; }
        public Answer Answer { get; set; }
    }
}
