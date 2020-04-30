using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            QuizQuestionVM vm = new QuizQuestionVM(quizId, questionRepo);
            return View(vm);
        }

        // GET: Question/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }

            var result = await questionRepo.GetQuestionByIdAsync(id.Value);

            if (result == null)
            {
                return Redirect("/ErrorPage/404");
            }
            return View(result);
        }

        // GET: Question/Create
        public async Task<ActionResult> Create(Guid? id)
        {
            if (id == null)
            {
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }

            return View(new QuizQuestionVM(id ?? Guid.Empty));
        }

        // POST: Question/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection, QuizQuestionVM quizQuestionVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(quizQuestionVM);
                }

                // Check if question exists

                // Get all questions with the same questionstring (1)
                var result = await questionRepo.GetAllQuestionsByQuestionAsync(quizQuestionVM.Question.QuestionString);

                Question question= new Question();
                QuizzesQuestions quizzesQuestions = new QuizzesQuestions();

                // Check if result contains rows
                if (result.Count() == 0)
                {
                    // If not ... insert question and connect in second table
                    question = await questionRepo.Add(quizQuestionVM.Question);
                    quizzesQuestions = await quizRepo.AddQuestionToQuiz(quizQuestionVM.QuizId, quizQuestionVM.Question.Id);
                }
                else
                {
                    // If result > 0

                    // Get the question from the database
                    question = result.ToList()[0];

                    // Check if question already in current quiz
                    var quizzes = await quizRepo.GetAllQuizzesByQuestionAsync(question.Id);

                    bool exists = false;

                    foreach (var item in quizzes)
                    {
                        if (item.QuizId == quizQuestionVM.QuizId)
                        {
                            exists = true;
                        }
                    }

                    if (exists)
                    {
                        // Error if already exists in current Quiz
                        ModelState.AddModelError("", "Question is already in your quiz");
                        return View(quizQuestionVM);
                    }
                    else
                    {
                        // If not exists ... add the existing question with quizid in second table
                        quizzesQuestions = await quizRepo.AddQuestionToQuiz(quizQuestionVM.QuizId, question.Id);
                    }
                }

                if (question == null)
                {
                    throw new Exception("Invalid Entry");
                }

                if (quizzesQuestions == null)
                {
                    throw new Exception("Invalid Entry");
                }

                return RedirectToAction(nameof(Index), new { quizId = quizQuestionVM.QuizId });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Create is giving an error: " + ex.Message);
                ModelState.AddModelError("", "Create action failed: " + ex.Message);
                return View(quizQuestionVM);
            }
        }

        // GET: Question/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }

            var result = await questionRepo.GetQuestionByIdAsync(id.Value);

            if (result == null)
            {
                return Redirect("/ErrorPage/404");
            }

            return View(result);
        }

        // POST: Question/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(IFormCollection collection, Question question)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Validation Error");
                }

                if (question.Id == null)
                {
                    return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
                }

                var result = await questionRepo.Update(question);

                if (result == null)
                {
                    return Redirect("/ErrorPage/404");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Update not succeeded: " + ex.Message.ToString());
                return View(question);
            }
        }

        // GET: Question/Delete/5
        public async Task<ActionResult> Delete(Guid? quizId, Guid? questionId)
        {
            if (quizId == null || questionId == null)
            {
                return View();
            }

            var result = await questionRepo.GetQuestionByIdAsync(questionId.Value);

            if (result == null)
            {
                ModelState.AddModelError("", "Not Found");
            }

            QuizQuestionVM quizQuestionVM = new QuizQuestionVM() { QuizId = quizId ?? Guid.Empty, Question = result };

            return View(quizQuestionVM);
        }

        // POST: Question/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(IFormCollection collection, QuizQuestionVM quizQuestionVM)
        {
            try
            {
                if (quizQuestionVM.QuizId == null || quizQuestionVM.Question.Id == null)
                {
                    return View();
                }

                // Get all answers
                var answers = await answerRepo.GetAllAnswersByQuestionAsync(quizQuestionVM.Question.Id);

                // Delete all answers

                QuestionsAnswers questionAnswer = null;
                Answer answer = null;

                foreach (var i in answers)
                {
                    var questions = await questionRepo.GetAllQuestionsByAnswerAsync(i.Id);

                    questionAnswer = null;
                    answer = null;

                    if (questions.Count() > 1)
                    {
                        // If the count is more than 1 ... only delete second table
                        foreach (var k in questions)
                        {
                            if (k.QuestionId == quizQuestionVM.Question.Id)
                            {
                                questionAnswer = await answerRepo.DeleteAnswerFromQuestion(k);

                                if (questionAnswer == null)
                                {
                                    return Redirect("/ErrorPage/404");
                                }
                            }
                        }
                    }
                    else if (questions.Count() == 1)
                    {
                        // If the answer is used in only one question ... delete second table and answer

                        answer = await answerRepo.Delete(i);

                        if (answer == null)
                        {
                            return Redirect("/ErrorPage/404");
                        }
                    }
                }
                

                // Check if question is used in more than 1 quizzes

                var quizzes = await quizRepo.GetAllQuizzesByQuestionAsync(quizQuestionVM.Question.Id);

                QuizzesQuestions quizzesQuestions = null;
                Question question = null;

                if (quizzes.Count() > 1)
                {
                    // If the count is more than 1 ... only delete second table
                    foreach (var item in quizzes)
                    {
                        if (item.QuizId == quizQuestionVM.QuizId)
                        {
                            quizzesQuestions = await questionRepo.DeleteQuestionFromQuiz(item);
                        }
                    }

                }
                else if (quizzes.Count() == 1)
                {
                    // If the answer is used in only one question ... delete second table and answer
                    var item = quizzes.ToList()[0];
                    quizzesQuestions = await questionRepo.DeleteQuestionFromQuiz(item);
                    question = await questionRepo.Delete(quizQuestionVM.Question);

                    if (question == null)
                    {
                        return Redirect("/ErrorPage/404");
                    }
                }
                else
                {
                    return Redirect("/ErrorPage/404");
                }

                if (quizzesQuestions == null)
                {
                    return Redirect("/ErrorPage/404");
                }

                return RedirectToAction(nameof(Index), new { quizId = quizQuestionVM.QuizId});
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Delete not succeeded: " + ex.Message.ToString());
                return View(quizQuestionVM);
            }
        }
    }
}