using System;
using System.Collections.Generic;
using System.Text;

namespace Quiztopia.Models
{
    public class QuizzesQuestions
    {
        // Properties

        public int QuizId { get; set; }
        public int QuestionId { get; set; }

        // Navigation properties

        public Quiz Quiz { get; set; }
        public Question Question { get; set; }
    }
}
