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
                context.Questions.AddAsync(question);
                await context.SaveChangesAsync();

                return question;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<QuestionsAnswers> AddAnswerToQuestion(QuestionsAnswers questionsAnswers)
        {
            try
            {
                context.QuestionsAnswers.AddAsync(questionsAnswers);
                await context.SaveChangesAsync();

                return questionsAnswers;
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
        public async Task<QuizzesQuestions> DeleteQuestionFromQuiz(QuizzesQuestions quizzesQuestions)
        {
            try
            {
                context.QuizzesQuestions.Remove(quizzesQuestions);
                await context.SaveChangesAsync();
                return quizzesQuestions;
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

        public async Task<Question> GetQuestionByIdAsync(Guid questionId)
        {
            return await context.Questions.SingleOrDefaultAsync<Question>(e => e.Id == questionId);
        }

        public async Task<IEnumerable<Question>> GetAllQuestionsByQuestionAsync(string question)
        {
            return await context.Questions.Where(e => e.QuestionString == question).OrderBy(e => e.QuestionString).ToListAsync();
        }

        public async Task<IEnumerable<QuestionsAnswers>> GetAllQuestionsByAnswerAsync(Guid answerId)
        {
            return await context.QuestionsAnswers.Where(e => e.AnswerId == answerId).ToListAsync();
        }

        public async Task<IEnumerable<Question>> GetAllQuestionsByQuizAsync(Guid quizId)
        {
            return await context.Questions.Include(q => q.QuizzesQuestions).ThenInclude(q => q.Quiz).Where(q => q.QuizzesQuestions.Any(i => i.Quiz.Id == quizId)).OrderBy(e => e.QuestionString).ToListAsync();
        }

        
    }
}
