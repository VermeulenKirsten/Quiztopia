using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Quiztopia.Models
{
    public class Quiz
    {
        // Properties

        [Key]
        [Display(Name = "Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Name")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Your quizname can only be 50 characters long.")]
        [Required(ErrorMessage = "The field 'Name' is required")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [MaxLength(500)]
        [StringLength(500, ErrorMessage = "Your description can only be 500 characters long.")]
        public string Description { get; set; } = "No description available";

        [Display(Name = "Topic")]
        public int TopicId { get; set; }

        [Display(Name = "Difficulty")]
        public int DifficultyId { get; set; }


        // Navigation properties

        public ICollection<QuizzesQuestions> QuizzesQuestions { get; set; }
        public Topic Topic { get; set; }
        public Difficulty Difficulty { get; set; }
    }
}
