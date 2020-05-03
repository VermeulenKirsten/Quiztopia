using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Quiztopia.Models;
using Quiztopia.Models.Repositories;
using Quiztopia.Web.Models;
using Quiztopia.Web.ViewModels;

namespace Quiztopia.Web.Controllers
{
    public class QuizController : Controller
    {
        private readonly IQuizRepo quizRepo;
        private readonly ITopicRepo topicRepo;
        private readonly IDifficultyRepo difficultyRepo;
        private readonly IQuestionRepo questionRepo;
        private readonly IAnswerRepo answerRepo;
        private readonly IScoreboardRepo scoreboardRepo;
        private readonly UserManager<IdentityUser> userMgr;

        public QuizController(IQuizRepo quizRepo, ITopicRepo topicRepo, IDifficultyRepo difficultyRepo, IQuestionRepo questionRepo, IAnswerRepo answerRepo, IScoreboardRepo scoreboardRepo, UserManager<IdentityUser> userMgr)
        {
            this.quizRepo = quizRepo;
            this.topicRepo = topicRepo;
            this.difficultyRepo = difficultyRepo;
            this.questionRepo = questionRepo;
            this.answerRepo = answerRepo;
            this.scoreboardRepo = scoreboardRepo;
            this.userMgr = userMgr;
        }

        // GET: Quiz
        public async Task<ActionResult> Index()
        {
            var result = await quizRepo.GetAllQuizzesAsync();
            return View(result);
        }

        // GET: Quiz/Play
        public async Task<ActionResult> Play(Guid? quizId)
        {
            Dictionary<Question, List<Answer>> questionAnswer = new Dictionary<Question, List<Answer>>();

            if (quizId == null)
            {
                return Redirect("/ErrorPage/404");
            }

            var quiz = await quizRepo.GetQuizByIdAsync(quizId ?? Guid.Empty);

            var questions = await questionRepo.GetAllQuestionsByQuizAsync(quizId ?? Guid.Empty);

            foreach (var question in questions)
            {
                var answer = await answerRepo.GetAllAnswersByQuestionAsync(question.Id);
                questionAnswer.Add(question, answer.ToList());
            }


            return View(new PlayVM() {UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), Quiz = quiz, QuestionAnswers = questionAnswer});
        }

        // POST: Quiz/Play
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Play(IFormCollection collection, PlayVM vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                // Get current IdentityUser
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Get current quiz
                var quiz = await quizRepo.GetQuizByIdAsync(vm.Quiz.Id);

                var questions = 0;
                var points = 0;
                var listQuestions = new List<string>();
                var listAnswers = new List<string>();
                // Calculate Score
                foreach (var instance in collection)
                {
                    if (questions <= collection.Count - 5)
                    {
                        listQuestions.Add(instance.Key);
                        listAnswers.Add(collection[instance.Key]);

                        if (collection[instance.Key] == "True")
                        {
                            points += 1;
                        }
                    }
                    else
                    {
                        break;
                    }

                    questions += 1;
                }

                // Create instance of Scoreboard

                var score = new Scoreboard()
                {
                    Id = Guid.NewGuid(),
                    YourScore = points,
                    TotalScore = questions,
                    UserId = Guid.Parse(userId)
                };

                // Insert instance of Scoreboard

                var scoreboard = await scoreboardRepo.Add(score);

                var in_between = await scoreboardRepo.AddScoreToQuiz(new QuizzesScoreboards() { QuizId = vm.Quiz.Id, ScoreboardId = scoreboard.Id });

                if (scoreboard == null || in_between == null)
                {
                    throw new Exception("Invalid Entry");
                }

                // Gather Answers and redirect to Overview


                OverviewVM overviewVM = new OverviewVM() { Questions = listQuestions, Answers = listAnswers };

                TempData["GivenAnswer"] = JsonConvert.SerializeObject(overviewVM);

                return RedirectToAction(nameof(Overview));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Create is giving an error: " + ex.Message);
                ModelState.AddModelError("", "Create action failed: " + ex.Message);
                return View(vm);
            }
        }

        // GET: Quiz/Overview
        public async Task<ActionResult> Overview()
        {
            var questionsAnswers = JsonConvert.DeserializeObject<OverviewVM>(TempData["GivenAnswer"].ToString());
            return View(questionsAnswers);
        }

        // GET: Quiz/Scoreboard
        public async Task<ActionResult> Scores(Guid? quizId)
        {
            if (quizId == null)
            {
                return Redirect("/ErrorPage/404");
            }

            var result = await quizRepo.GetQuizByIdAsync(quizId ?? Guid.Empty);

            var user = await userMgr.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            ScoreboardVM vm = new ScoreboardVM() { Quiz = result, User = user };

            return View(vm);
        }

        // GET: Quiz/Create
        public async Task<ActionResult> Create()
        {
            return View(new QuizVM(new Quiz(), topicRepo, difficultyRepo));
        }

        // POST: Quiz/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection, QuizVM vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                var result = await quizRepo.Add(vm.Quiz);

                if (result == null)
                {
                    throw new Exception("Invalid Entry");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Create is giving an error: " + ex.Message);
                ModelState.AddModelError("", "Create action failed: " + ex.Message);
                return View(vm);
            }
        }

        // GET: Quiz/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }

            var result = await quizRepo.GetQuizByIdAsync(id.Value);

            if (result == null)
            {
                return Redirect("/ErrorPage/404");
            }

            return View(new QuizVM(result, topicRepo, difficultyRepo));
        }

        // POST: Quiz/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid? id, IFormCollection collection, QuizVM vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Validation Error");
                }

                if (id == null)
                {
                    return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
                }

                vm.Quiz.Id = id ?? Guid.Empty;

                var result = await quizRepo.Update(vm.Quiz);

                if (result == null)
                {
                    return Redirect("/ErrorPage/404");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Update not succeeded: " + ex.Message.ToString());
                return View(vm);
            }
        }

        // GET: Quiz/Delete/5
        public async Task<ActionResult> Delete(Guid? quizId)
        {
            if (quizId == null)
            {
                return Redirect("/ErrorPage/400");
            }

            var result = await quizRepo.GetQuizByIdAsync(quizId.Value);

            if (result == null)
            {
                return Redirect("/ErrorPage/404");
            }

            return View(result);
        }

        // POST: Quiz/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(IFormCollection collection, Quiz quiz)
        {
            try
            {
                if (quiz.Id == null)
                {
                    return Redirect("/ErrorPage/400");
                }

                var result = await quizRepo.Delete(quiz);

                if (result == null)
                {
                    return Redirect("/ErrorPage/404");
                }

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Delete not succeeded: " + ex.Message.ToString());
                return View(quiz);
            }
        }


        // GET: Quiz/DeleteQuestions/5
        public async Task<ActionResult> DeleteQuestions(Guid? quizId)
        {
            if (quizId == null)
            {
                return Redirect("/ErrorPage/400");
            }

            var result = await quizRepo.GetQuizByIdAsync(quizId.Value);

            if (result == null)
            {
                return Redirect("/ErrorPage/404");
            }

            return View(result);
        }

        // POST: Quiz/DeleteQuestions/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteQuestions(IFormCollection collection, Quiz quiz)
        {
            try
            {
                if (quiz.Id == null)
                {
                    return Redirect("/ErrorPage/400");
                }

                // Get all questions form quiz

                var questions = await questionRepo.GetAllQuestionsByQuizAsync(quiz.Id);

                foreach (var question in questions)
                {
                    // Check if question is in another quiz
                    var questionQuizzes = await quizRepo.GetQuizzesByQuestionAsync(question.Id);

                    if (questionQuizzes.Count() > 1)
                    {
                        // Delete table in between

                        foreach (var item in questionQuizzes)
                        {
                            if (item.QuizId == quiz.Id)
                            {
                                var questionQuiz = await questionRepo.DeleteQuestionFromQuiz(item);

                                if (questionQuiz == null)
                                {
                                    return Redirect("/ErrorPage/404");
                                }
                            }
                        }
                    }
                    else if (questionQuizzes.Count() == 1)
                    {
                        // Get all answers from question
                        var answers = await answerRepo.GetAllAnswersByQuestionAsync(question.Id);

                        // Delete answers
                        if (answers.Count() >= 1)
                        {
                            foreach (var item in answers)
                            {
                                var deleted_answer = await answerRepo.Delete(item);

                                if (deleted_answer == null)
                                {
                                    return Redirect("/ErrorPage/404");
                                }
                            }
                        }

                        // Delete Question
                        var deletedQuestion = await questionRepo.Delete(question);

                        if (deletedQuestion == null)
                        {
                            return Redirect("/ErrorPage/404");
                        }

                    }
                    else
                    {
                        return Redirect("/ErrorPage/404");
                    }
                }

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Delete not succeeded: " + ex.Message.ToString());
                return View(quiz);
            }
        }
    }
}