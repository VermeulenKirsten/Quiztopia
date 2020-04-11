using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Quiztopia.Models.Data
{
    public class QuiztopiaDbContext : IdentityDbContext<IdentityUser>
    {
        // Constructor

        public QuiztopiaDbContext(DbContextOptions<QuiztopiaDbContext> options)
            : base(options)
        {
        }

        // Properties

        public virtual DbSet<Quiz> Quizzes { get; set; }
        public virtual DbSet<QuizzesQuestions> QuizzesQuestions { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionsAnswers> QuestionsAnswers { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<Difficulty> Difficulties{ get; set; }

        // Fluent API

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QuizzesQuestions>(entity =>
            {
                entity.HasKey(e => new { e.QuizId, e.QuestionId });
            });

            modelBuilder.Entity<QuestionsAnswers>(entity =>
            {
                entity.HasKey(e => new { e.QuestionId, e.AnswerId });
            });

        }
    }
}
