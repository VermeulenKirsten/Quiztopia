using Microsoft.EntityFrameworkCore;
using Quiztopia.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiztopia.Models.Repositories
{
    public class AnswerRepo : IAnswerRepo
    {
        private readonly QuiztopiaDbContext context;

        public AnswerRepo(QuiztopiaDbContext context)
        {
            this.context = context;
        }

        public async Task<Answer> Add(Answer answer)
        {
            try
            {
                var result = context.Answers.AddAsync(answer);
                await context.SaveChangesAsync();
                return answer;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Answer> Update(Answer answer)
        {
            try
            {
                context.Answers.Update(answer);
                await context.SaveChangesAsync();
                return answer;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Answer> Delete(Answer answer)
        {
            try
            {
                context.Answers.Remove(answer);
                await context.SaveChangesAsync();
                return answer;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<Answer>> GetAllAnswersAsync()
        {
            return await context.Answers.OrderBy(e => e.AnswerString).ToListAsync();
        }

        public async Task<Answer> GetAnswerByIdAsync(int answerId)
        {
            return await context.Answers.SingleOrDefaultAsync<Answer>(e => e.Id == answerId);
        }

        public async Task<IEnumerable<Answer>> GetAllAnswersByQuestionAsync(int questionId)
        {
            throw new NotImplementedException();
        }

    }
}
