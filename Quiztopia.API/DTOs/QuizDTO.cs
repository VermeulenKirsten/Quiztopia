using Quiztopia.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quiztopia.API.DTOs
{
    public class QuizDTO
    {
        [Display(Name = "Name")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Your quizname can only be 50 characters long.")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Topic { get; set; } = "General Knowledge";

        public string Difficulty { get; set; } = "Easy";

        [Display(Name = "Description")]
        [MaxLength(500)]
        [StringLength(500, ErrorMessage = "Your description can only be 500 characters long.")]
        public string Description { get; set; } = "No descriptions available";
    }
}
