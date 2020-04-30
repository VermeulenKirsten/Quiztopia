using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Quiztopia.Models
{
    public class Question
    {
        // Properties
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Question")]
        [StringLength(50, ErrorMessage = "Please, use less than 100 characters")]
        [MaxLength(50)]
        [Required(ErrorMessage = "Question is required")]
        public string QuestionString { get; set; }


        // Navigation properties

        public ICollection<QuizzesQuestions> QuizzesQuestions { get; set; }
        public ICollection<QuestionsAnswers> QuestionsAnswers { get; set; }
    }
}
