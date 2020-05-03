using Microsoft.EntityFrameworkCore;
using Quiztopia.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiztopia.Models.Repositories
{
    public class QuizRepo : IQuizRepo
    {
        private readonly QuiztopiaDbContext context;

        public QuizRepo(QuiztopiaDbContext context)
        {
            this.context = context;
        }

        public async Task<Quiz> Add(Quiz quiz)
        {
            try
            {
                var result = context.Quizzes.AddAsync(quiz);
                await context.SaveChangesAsync();
                return quiz;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Quiz> Update(Quiz quiz)
        {
            try
            {
                context.Quizzes.Update(quiz);
                await context.SaveChangesAsync();
                return quiz;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Quiz> Delete(Quiz quiz)
        {
            try
            {
                context.Quizzes.Remove(quiz);
                await context.SaveChangesAsync();
                return quiz;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<Quiz>> GetAllQuizzesAsync()
        {
            return await context.Quizzes.Include(t => t.Topic).Include(d => d.Difficulty).Include(t => t.QuizzesQuestions).ThenInclude(t => t.Question).ThenInclude(t => t.Answers).OrderBy(q => q.Name).ToListAsync();
        }

        public async Task<IEnumerable<Quiz>> GetQuizzesByNameAsync(string name)
        {
            return await context.Quizzes.Include(t => t.Topic).Include(d => d.Difficulty).Where(n => n.Name.Contains(name)).OrderBy(q => q.Name).ToListAsync();
        }

        public async Task<Quiz> GetQuizByIdAsync(Guid quizId)
        {
            return await context.Quizzes.Include(s => s.QuizzesScoreboards).ThenInclude(s => s.Scoreboard).SingleOrDefaultAsync<Quiz>(e => e.Id == quizId);
        }

        public async Task<IEnumerable<QuizzesQuestions>> GetQuizzesByQuestionAsync(Guid questionId)
        {
            return await context.QuizzesQuestions.Include(q => q.Question).Where(q => q.Question.Id == questionId).ToListAsync();
        }
    }
}
