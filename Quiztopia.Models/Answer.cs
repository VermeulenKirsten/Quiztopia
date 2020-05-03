using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Quiztopia.Models
{
    public class Answer
    {
        // Properties

        [Key]
        [Display(Name = "Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Display(Name = "Answer")]
        [MaxLength(50)]
        [StringLength(50, ErrorMessage = "Your answer can only be 50 characters long.")]
        [Required(ErrorMessage = "Answer is required")]
        public string PossibleAnswer { get; set; }

        [Display(Name = "Correct")]
        public bool IsCorrect { get; set; }

        // Navigation properties

        public Question Question { get; set; }
    }
}
