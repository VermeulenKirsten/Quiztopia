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
        public async Task<ActionResult> Index(Guid questionId, Guid quizId)
        {
            var answers = await answerRepo.GetAllAnswersByQuestionAsync(questionId);
            var question = await questionRepo.GetQuestionByIdAsync(questionId);

            return View(new QuestionAnswerVM() { QuizId = quizId, Question = question, Answers = answers.ToList() });
        }

        // GET: Answer/Create
        public async Task<ActionResult> Create(Guid? questionId, Guid? quizId)
        {
            if(questionId == null || quizId == null)
            {
                return Redirect("/ErrorPage/400");
            }

            var question = await questionRepo.GetQuestionByIdAsync(questionId ?? Guid.Empty);

            if (question == null)
            {
                return Redirect("/ErrorPage/404");
            }

            return View(new QuestionAnswerVM() {QuizId = quizId ?? Guid.Empty, Question = question, Answer = new Answer() { Id = Guid.NewGuid() } });
        }

        // POST: Answer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection, QuestionAnswerVM vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                var question = await questionRepo.GetQuestionByIdAsync(vm.Question.Id);

                if (question == null)
                {
                    return Redirect("/ErrorPage/404");
                }

                vm.Answer.Question = question;

                var result = await answerRepo.Add(vm.Answer);

                if (result == null)
                {
                    throw new Exception("Invalid Entry");
                }

                return RedirectToAction(nameof(Index), new { questionId = vm.Question.Id, quizId = vm.QuizId});
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Create is giving an error: " + ex.Message);
                ModelState.AddModelError("", "Create action failed: " + ex.Message);
                return View(vm);
            }
        }

        // GET: Answer/Edit/5
        public async Task<ActionResult> Edit(Guid? answerId, Guid? questionId, Guid? quizId)
        {
            if (answerId == null || questionId == null || quizId == null)
            {
                return Redirect("/ErrorPage/400");
            }

            var result = await answerRepo.GetAnswerByIdAsync(answerId.Value);

            if (result == null)
            {
                return Redirect("/ErrorPage/404");
            }

            var question = await questionRepo.GetQuestionByIdAsync(questionId ?? Guid.Empty);

            if (result == null)
            {
                return Redirect("/ErrorPage/404");
            }

            return View(new QuestionAnswerVM() { QuizId = quizId ?? Guid.Empty, Question = question, Answer = result });
        }

        // POST: Answer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(IFormCollection collection, QuestionAnswerVM vm)
        {
            try
            {
                if (vm.Answer.Id == null || vm.QuizId == null || vm.Question.Id == null)
                {
                    return Redirect("/ErrorPage/400");
                }

                if (!ModelState.IsValid)
                {
                    return View(vm.Answer);
                }

                var result = await answerRepo.Update(vm.Answer);

                if (result == null)
                {
                    return Redirect("/ErrorPage/404");
                }

                return RedirectToAction(nameof(Index), new { questionId = vm.Question.Id, quizId = vm.QuizId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Update not succeeded: " + ex.Message.ToString());
                return View(vm.Answer);
            }
        }

        // GET: Answer/Delete/5
        public async Task<ActionResult> Delete(Guid? answerId, Guid? questionId, Guid? quizId)
        {
            if (answerId == null || questionId == null || quizId == null)
            {
                return Redirect("/ErrorPage/400");
            }

            var result = await answerRepo.GetAnswerByIdAsync(answerId.Value);

            if (result == null)
            {
                ModelState.AddModelError("", "Not Found");
            }

            var question = await questionRepo.GetQuestionByIdAsync(questionId ?? Guid.Empty);

            QuestionAnswerVM vm = new QuestionAnswerVM() { QuizId = quizId ?? Guid.Empty, Question = question, Answer = result };

            return View(vm);
        }

        // POST: Answer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(IFormCollection collection, QuestionAnswerVM vm)
        {
            try
            {
                if (vm.Answer.Id == null || vm.QuizId == null || vm.Question == null)
                {
                    return Redirect("/ErrorPage/400");
                }

                var result = await answerRepo.Delete(vm.Answer);

                if (result == null)
                {
                    return Redirect("/ErrorPage/404");
                }

                return RedirectToAction(nameof(Index), new { questionId = vm.Question.Id, quizId = vm.QuizId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Delete not succeeded: " + ex.Message.ToString());
                return View(vm.Answer);
            }
        }
    }
}