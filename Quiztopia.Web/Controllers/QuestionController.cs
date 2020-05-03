using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quiztopia.Models;
using Quiztopia.Models.Repositories;
using Quiztopia.Web.Models;
using Quiztopia.Web.ViewModels;

namespace Quiztopia.Web.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IQuestionRepo questionRepo;
        private readonly IQuizRepo quizRepo;
        private readonly IAnswerRepo answerRepo;

        public QuestionController(IQuestionRepo questionRepo, IQuizRepo quizRepo, IAnswerRepo answerRepo)
        {
            this.questionRepo = questionRepo;
            this.quizRepo = quizRepo;
            this.answerRepo = answerRepo;
        }

        // GET: Question
        public async Task<ActionResult> Index(Guid quizId)
        {
            var result = await questionRepo.GetAllQuestionsByQuizAsync(quizId);
            return View(new QuizQuestionVM() { QuizId = quizId, Questions = result.ToList() });
        }

        // GET: Question/Create
        public async Task<ActionResult> Create(Guid quizId)
        {
            return View(new QuizQuestionVM() { QuizId = quizId, Question = new Question() });
        }

        // POST: Question/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection, QuizQuestionVM vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                if (vm.Question.ImageString != null)
                {
                    byte[] b;
                    using (BinaryReader br = new BinaryReader(vm.Question.ImageString.OpenReadStream()))
                    {
                        b = br.ReadBytes((int)vm.Question.ImageString.OpenReadStream().Length);
                        vm.Question.ImageData = b;
                        // Convert the image in to bytes
                    }
                }

                // Check if question already exists

                var questions = await questionRepo.GetQuestionByQuestionAsync(vm.Question.QuestionString);

                if(questions.Count() >= 1)
                {
                    bool exists = false;
                    
                    foreach (var item in questions)
                    {
                        // Check if question already in quiz

                        var quizzesQuestions = await questionRepo.GetAllQuizzesQuestionsAsync(item.Id);

                        exists = false;

                        foreach (var instance in quizzesQuestions)
                        {
                            if(instance.QuizId == vm.QuizId)
                            {
                                exists = true;
                            }
                        }

                        if (!exists)
                        {
                            // Add to table in between
                            var in_between = await questionRepo.AddQuestionToQuiz(new QuizzesQuestions() { QuestionId = item.Id, QuizId = vm.QuizId });

                            if (in_between == null)
                            {
                                throw new Exception("Invalid Entry");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "This question already exists in this quiz");
                            return View(vm);
                        }
                    }
                }
                else
                {
                    var question = await questionRepo.Add(vm.Question);

                    var in_between = await questionRepo.AddQuestionToQuiz(new QuizzesQuestions() { QuestionId = vm.Question.Id, QuizId = vm.QuizId });

                    if (question == null || in_between == null)
                    {
                        throw new Exception("Invalid Entry");
                    }
                }

                return RedirectToAction(nameof(Index), new { quizId = vm.QuizId });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Create is giving an error: " + ex.Message);
                ModelState.AddModelError("", "Create action failed: " + ex.Message);
                return View(vm);
            }
        }

        // GET: Question/Edit/5
        public async Task<ActionResult> Edit(Guid? questionId, Guid? quizId)
        {
            if (questionId == null || quizId == null)
            {
                return Redirect("/ErrorPage/400");
            }

            var result = await questionRepo.GetQuestionByIdAsync(questionId.Value);

            if (result == null)
            {
                return Redirect("/ErrorPage/404");
            }

            return View(new QuizQuestionVM() { QuizId = quizId ?? Guid.Empty, Question = result });
        }

        // POST: Question/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(IFormCollection collection, QuizQuestionVM vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Validation Error");
                }

                if (vm.Question.Id == null || vm.QuizId == null)
                {
                    return Redirect("/ErrorPage/400");
                }

                var result = await questionRepo.Update(vm.Question);

                if (result == null)
                {
                    return Redirect("/ErrorPage/404");
                }

                return RedirectToAction(nameof(Index), new { quizId = vm.QuizId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Update not succeeded: " + ex.Message.ToString());
                return View(vm);
            }
        }

        // GET: Question/Delete/5
        public async Task<ActionResult> Delete(Guid? quizId, Guid? questionId)
        {
            if (quizId == null || questionId == null)
            {
                return Redirect("/ErrorPage/400");
            }

            var result = await questionRepo.GetQuestionByIdAsync(questionId.Value);

            if (result == null)
            {
                ModelState.AddModelError("", "Not Found");
            }

            QuizQuestionVM vm = new QuizQuestionVM() { QuizId = quizId ?? Guid.Empty, Question = result };

            return View(vm);
        }

        // POST: Question/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(IFormCollection collection, QuizQuestionVM vm)
        {
            try
            {
                if (vm.QuizId == null || vm.Question.Id == null)
                {
                    return Redirect("/ErrorPage/400");
                }

                var question = await questionRepo.GetQuestionByIdAsync(vm.Question.Id);

                // Check if question is in another quiz
                var questionQuizzes = await quizRepo.GetQuizzesByQuestionAsync(question.Id);

                if (questionQuizzes.Count() > 1)
                {
                    // Delete table in between

                    foreach (var item in questionQuizzes)
                    {
                        if (item.QuizId == vm.QuizId)
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
                    var answers = await answerRepo.GetAllAnswersByQuestionAsync(vm.Question.Id);

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
                    var result = await questionRepo.Delete(question);

                    if (result == null)
                    {
                        return Redirect("/ErrorPage/404");
                    }

                }
                else
                {
                    return Redirect("/ErrorPage/404");
                }

                return RedirectToAction(nameof(Index), new { quizId = vm.QuizId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Delete not succeeded: " + ex.Message.ToString());
                return View(vm);
            }
        }
    }
}