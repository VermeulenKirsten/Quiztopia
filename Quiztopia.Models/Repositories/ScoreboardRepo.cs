using Quiztopia.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quiztopia.Models.Repositories
{
    public class ScoreboardRepo : IScoreboardRepo
    {
        private readonly QuiztopiaDbContext context;

        public ScoreboardRepo(QuiztopiaDbContext context)
        {
            this.context = context;
        }

        // Add 
        public async Task<Scoreboard> Add(Scoreboard scoreboard)
        {
            try
            {
                var result = context.Scoreboards.AddAsync(scoreboard);
                await context.SaveChangesAsync();
                return scoreboard;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<QuizzesScoreboards> AddScoreToQuiz(QuizzesScoreboards quizzesScoreboards)
        {
            try
            {
                var result = context.QuizzesScoreboards.AddAsync(quizzesScoreboards);
                await context.SaveChangesAsync();
                return quizzesScoreboards;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
