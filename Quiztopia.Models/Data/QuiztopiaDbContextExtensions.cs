using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiztopia.Models.Data
{
    public static class QuiztopiaDbContextExtensions
    {
        private static List<Difficulty> _difficulties = new List<Difficulty>
        {
            new Difficulty { Name = "Very Easy" },
            new Difficulty { Name = "Easy" },
            new Difficulty { Name = "Intermediate" },
            new Difficulty { Name = "Hard" },
            new Difficulty { Name = "Very Hard" },

        };

        private static List<Topic> _topics = new List<Topic>
        {
            new Topic { Name = "General Knowledge" },
            new Topic { Name = "Music" },
            new Topic { Name = "Sports" },
            new Topic { Name = "Film" },
            new Topic { Name = "Food and Drink" },
            new Topic { Name = "Geography" },
            new Topic { Name = "History" },
            new Topic { Name = "Math" },
            new Topic { Name = "Language" },
            new Topic { Name = "Science" },
            new Topic { Name = "Animals" },
            new Topic { Name = "Friends and Family" },
            
        };

        public async static Task SeedRoles(RoleManager<IdentityRole> RoleMgr)
        {
            IdentityResult roleResult;
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                // Only add if not exist
                var roleExist = await RoleMgr.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleMgr.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public async static Task SeedUsers(UserManager<IdentityUser> userMgr)
        {
            // ---------- 1. Admin aanmaken ----------

            if (await userMgr.FindByNameAsync("Docent@MCT") == null)        
            {
                var user = new IdentityUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Docent@MCT",
                    Email = "docent@howest.be"
                };

                var userResult = await userMgr.CreateAsync(user, "Docent@1");
                var roleResult = await userMgr.AddToRoleAsync(user, "Admin");

                if (!userResult.Succeeded || !roleResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user and roles");
                }
            }

            // ---------- 2. meerdere users  aanmaken ----------

            var nmbrStudents = 5;

            for (var i = 1; i <= nmbrStudents; i++)
            {
                if (userMgr.FindByNameAsync("User@" + i).Result == null)
                {
                    IdentityUser user = new IdentityUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = ("User@" + i).Trim(),
                        Email = "User" + i + "@student.howest.be",                  
                    };

                    var userResult = await userMgr.CreateAsync(user, "User@" + i);
                    var roleResult = await userMgr.AddToRoleAsync(user, "User");

                    if (!userResult.Succeeded || !roleResult.Succeeded)
                    {
                        throw new InvalidOperationException("Failed to build " + user.UserName);
                    }
                }
            }
        }
        public async static Task SeedData(QuiztopiaDbContext context)
        {
            //1. Topics          

            if (!context.Topics.Any())
            {                
                Debug.WriteLine("Seeding Topics");

                foreach (Topic t in _topics)
                {
                    if (!context.Topics.Any(i => i.Id == t.Id))
                        await context.Topics.AddAsync(t);
                }

                await context.SaveChangesAsync();

            }

            //2. Difficulties          

            if (!context.Difficulties.Any())
            {               
                Debug.WriteLine("Seeding Difficulties");

                foreach (Difficulty d in _difficulties)
                {
                    if (!context.Difficulties.Any(i => i.Id == d.Id))
                        await context.Difficulties.AddAsync(d);
                }

                await context.SaveChangesAsync();

            }


            if (!context.Quizzes.Any())
            {

                // 3. Quiz

                Quiz quiz = new Quiz()
                {
                    Id = Guid.NewGuid(),
                    Name = "Johan",
                    Description = "This quiz is made via a data seeder",
                    Topic = await context.Topics.SingleOrDefaultAsync<Topic>(t => t.Name == "General Knowledge"),
                    Difficulty = await context.Difficulties.SingleOrDefaultAsync<Difficulty>(d => d.Name == "Easy")
                };

                await context.Quizzes.AddAsync(quiz);
                await context.SaveChangesAsync();

                // 4. Question

                Question question = new Question()
                {
                    Id = Guid.NewGuid(),
                    QuestionString = "Is Johan the best teacher?"
                };

                await context.Questions.AddAsync(question);

                // 5. QuizzesQuestion

                QuizzesQuestions quizzesQuestions = new QuizzesQuestions()
                {
                    QuizId = quiz.Id,
                    QuestionId = question.Id
                };

                await context.QuizzesQuestions.AddAsync(quizzesQuestions);

                // 6. Answers

                List<Answer> answers = new List<Answer>
                {
                    new Answer() { Id = Guid.NewGuid(), PossibleAnswer = "Yes", IsCorrect = true, Question = question},
                    new Answer() { Id = Guid.NewGuid(), PossibleAnswer = "No", IsCorrect = false, Question = question}
                };

                foreach (var answer in answers)
                {
                    if (!context.Answers.Any(i => i.Id == answer.Id))
                        await context.Answers.AddAsync(answer);
                }

                await context.SaveChangesAsync();

            }
        }
    }
}
