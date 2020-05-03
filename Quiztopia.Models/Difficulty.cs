using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Quiztopia.Models
{
    public class Difficulty
    {
        // Properties

        [Key]
        [Display(Name = "Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Difficulty")]
        [MaxLength(25)]
        [StringLength(25, ErrorMessage = "Your difficulty can only be 25 characters long.")]
        [Required(ErrorMessage = "Difficulty is required")]
        public string Name { get; set; }

        // Navigation properties

        public ICollection<Quiz> Quiz { get; set; }
    }
}
