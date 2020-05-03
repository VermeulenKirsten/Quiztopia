using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quiztopia.Models
{
    public class Scoreboard
    {
        // Properties

        public Guid Id { get; set; }
        public int YourScore { get; set; }
        public int TotalScore { get; set; }
        public Guid UserId { get; set; }

        // Navigation Properties
        public ICollection<QuizzesScoreboards> QuizzesScoreboards { get; set; }

    }
}
