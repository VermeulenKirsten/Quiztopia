using System;
using System.Collections.Generic;
using System.Text;

namespace Quiztopia.Models
{
    public class QuestionsAnswers
    {
        // Properties

        public int QuestionId { get; set; }
        public int AnswerId { get; set; }

        // Navigation properties

        public Question Question { get; set; }
        public Answer Answer { get; set; }
    }
}
