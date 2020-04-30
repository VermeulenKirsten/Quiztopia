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
            return await context.Quizzes.Include(t => t.Topic).Include(d => d.Difficulty).OrderBy(q => q.Name).ToListAsync();
        }

        public async Task<IEnumerable<Quiz>> GetQuizzesByNameAsync(string name)
        {
            return await context.Quizzes.Include(t => t.Topic).Include(d => d.Difficulty).Where(n => n.Name.Contains(name)).OrderBy(q => q.Name).ToListAsync();
        }

        public async Task<Quiz> GetQuizByIdAsync(int quizId)
        {
            return await context.Quizzes.SingleOrDefaultAsync<Quiz>(e => e.Id == quizId);
        }
    }
}
