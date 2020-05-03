using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Quiztopia.Models
{
    public class Question
    {
        // Properties

        [Key]
        [Display(Name = "Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Display(Name = "Question")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Your question can only be 50 characters long.")]
        [Required(ErrorMessage = "Question is required")]
        public string QuestionString { get; set; }

        [NotMapped]
        [Display(Name = "Image")]
        public IFormFile ImageString { get; set; }
        [Display(Name ="Image")]
        public byte[] ImageData { get; set; }


        // Navigation properties

        public ICollection<QuizzesQuestions> QuizzesQuestions { get; set; }

        public ICollection<Answer> Answers { get; set; }
    }
}
