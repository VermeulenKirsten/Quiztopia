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
        public string Name { get; set; }
        public string Topic { get; set; } = "General";
        public string Difficulty { get; set; } = "Easy";
        public string Descriptions { get; set; } = "No descriptions available";
    }
}
