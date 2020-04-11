using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Quiztopia.Models
{
    public class Topic
    {
        // Properties

        [Key]
        [Display(Name = "Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Name")]
        [MaxLength(25)]
        [StringLength(25, ErrorMessage = "Your answer can only be 25 characters long.")]
        [Required(ErrorMessage = "The field 'Name' is required")]
        public string Name { get; set; }
    }
}
