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
    public class AnswerController : Controller
    {
        private readonly IAnswerRepo answerRepo;
        private readonly IQuestionRepo questionRepo;

        public AnswerController(IAnswerRepo answerRepo, IQuestionRepo questionRepo)
        {
            this.answerRepo = answerRepo;
            this.questionRepo = questionRepo;
        }

        // GET: Answer
        public async Task<ActionResult> Index(Guid questionId)
        {
            QuestionAnswerVM vm = new QuestionAnswerVM(questionId, answerRepo);
            return View(vm);
        }

        // GET: Answer/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }

            var result = await answerRepo.GetAnswerByIdAsync(id.Value);

            if (result == null)
            {
                return Redirect("/ErrorPage/404");
            }
            return View(result);
        }

        // GET: Answer/Create
        public async Task<ActionResult> Create(Guid? id)
        {
            if (id == null)
            {
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }

            return View(new QuestionAnswerVM(id ?? Guid.Empty));
        }

        // POST: Answer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection, QuestionAnswerVM questionAnswerVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
                }

                // Check if answer exists

                var result = await answerRepo.GetAnswerByAnswerAsync(questionAnswerVM.Answer.PossibleAnswer, questionAnswerVM.Answer.IsCorrect);

                Answer answer = null;
                QuestionsAnswers questionAnswer = null;

                if (result.Count() == 0)
                {
                    answer = await answerRepo.Add(questionAnswerVM.Answer);
                    QuestionsAnswers questionsAnswers = new QuestionsAnswers() { QuestionId = questionAnswerVM.QuestionId, AnswerId = answer.Id };
                    questionAnswer = await questionRepo.AddAnswerToQuestion(questionsAnswers);
                }
                else
                {
                    answer = result.ToList()[0];
                    var questions = await questionRepo.GetAllQuestionsByAnswerAsync(answer.Id);

                    bool exists = false;

                    foreach (var item in questions)
                    {
                        if (item.QuestionId == questionAnswerVM.QuestionId)
                        {
                            exists = true;
                        }
                    }

                    if (exists)
                    {
                        ModelState.AddModelError("", "Answer is already in your question");
                        return View(questionAnswerVM);
                    }
                    else
                    {
                        QuestionsAnswers questionsAnswers = new QuestionsAnswers() { QuestionId = questionAnswerVM.QuestionId, AnswerId = answer.Id };
                        questionAnswer = await questionRepo.AddAnswerToQuestion(questionsAnswers);
                    }
                }

                if (answer == null)
                {
                    throw new Exception("Invalid Entry");
                }

                if (questionAnswer == null)
                {
                    throw new Exception("Invalid Entry");
                }

                return RedirectToAction(nameof(Index), new { questionId = questionAnswerVM.QuestionId });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Create is giving an error: " + ex.Message);
                ModelState.AddModelError("", "Create action failed: " + ex.Message);
                return View(questionAnswerVM);
            }
        }

        // GET: Answer/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }

            var result = await answerRepo.GetAnswerByIdAsync(id.Value);

            if (result == null)
            {
                return Redirect("/ErrorPage/404");
            }

            return View(result);
        }

        // POST: Answer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, IFormCollection collection, Answer answer)
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

                var result = await answerRepo.Update(answer);

                if (result == null)
                {
                    return Redirect("/ErrorPage/404");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Update not succeeded: " + ex.Message.ToString());
                return View(answer);
            }
        }

        // GET: Answer/Delete/5
        public async Task<ActionResult> Delete(Guid? questionId, Guid? answerId)
        {
            if (questionId == null || answerId == null)
            {
                return View();
            }

            var result = await answerRepo.GetAnswerByIdAsync(answerId.Value);

            if (result == null)
            {
                ModelState.AddModelError("", "Not Found");
            }

            QuestionAnswerVM questionAnswerVM = new QuestionAnswerVM() { QuestionId = questionId ?? Guid.Empty, Answer = result };

            return View(questionAnswerVM);
        }

        // POST: Answer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(IFormCollection collection, QuestionAnswerVM questionAnswerVM)
        {
            try
            {
                if (questionAnswerVM.QuestionId == null || questionAnswerVM.Answer.Id == null)
                {
                    return View(questionAnswerVM);
                }

                // Check if answer is used in more than 1 question

                var questions = await questionRepo.GetAllQuestionsByAnswerAsync(questionAnswerVM.Answer.Id);
                
                QuestionsAnswers questionAnswer = null;
                Answer answer = null;

                if (questions.Count() > 1)
                {
                    // If the count is more than 1 ... only delete second table
                    foreach (var item in questions)
                    {
                        if(item.QuestionId == questionAnswerVM.QuestionId)
                        {
                            questionAnswer = await answerRepo.DeleteAnswerFromQuestion(item);

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
                    
                    answer = await answerRepo.Delete(questionAnswerVM.Answer);

                    if (answer == null)
                    {
                        return Redirect("/ErrorPage/404");
                    }
                }
                else
                {
                    return Redirect("/ErrorPage/404");
                }

                return RedirectToAction(nameof(Index), new { questionId = questionAnswerVM.QuestionId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Delete not succeeded: " + ex.Message.ToString());
                return View(questionAnswerVM);
            }
        }
    }
}