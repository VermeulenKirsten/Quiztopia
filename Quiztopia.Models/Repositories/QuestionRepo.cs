using Microsoft.EntityFrameworkCore;
using Quiztopia.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiztopia.Models.Repositories
{
    public class QuestionRepo : IQuestionRepo
    {
        private readonly QuiztopiaDbContext context;

        public QuestionRepo(QuiztopiaDbContext context)
        {
            this.context = context;
        }

        public async Task<Question> Add(Question question)
        {
            try
            {
                var result = context.Questions.AddAsync(question);
                await context.SaveChangesAsync();
                return question;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Question> Update(Question question)
        {
            try
            {
                context.Questions.Update(question);
                await context.SaveChangesAsync();
                return question;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Question> Delete(Question question)
        {
            try
            {
                context.Questions.Remove(question);
                await context.SaveChangesAsync();
                return question;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<Question>> GetAllQuestionsAsync()
        {
            return await context.Questions.OrderBy(e => e.QuestionString).ToListAsync();
        }

        public async Task<Question> GetQuestionByIdAsync(int questionId)
        {
            return await context.Questions.SingleOrDefaultAsync<Question>(e => e.Id == questionId);
        }

        public async Task<IEnumerable<Question>> GetAllQuestionsByQuizAsync(int quizId)
        {
            throw new NotImplementedException();
        }
    }
}
