using System;
using System.Collections.Generic;
using System.Text;

namespace Quiztopia.Models
{
    public class Question
    {
        // Properties

        public int Id { get; set; }
        public string QuestionString { get; set; }


        // Navigation properties

        public ICollection<QuizzesQuestions> QuizzesQuestions { get; set; }
        public ICollection<QuestionsAnswers> QuestionsAnswers { get; set; }
    }
}
