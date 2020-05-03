using System;
using System.Collections.Generic;
using System.Text;

namespace Quiztopia.Models
{
    public class QuizzesScoreboards
    {

        // Properties

        public Guid QuizId { get; set; }
        public Guid ScoreboardId { get; set; }

        // Navigation properties

        public Quiz Quiz { get; set; }
        public Scoreboard Scoreboard { get; set; }
    }
}
