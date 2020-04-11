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

namespace Quiztopia.Web.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IQuestionRepo questionRepo;

        public QuestionController(IQuestionRepo questionRepo)
        {
            this.questionRepo = questionRepo;
        }

        // GET: Question
        public async Task<ActionResult> Index()
        {
            var result = await questionRepo.GetAllQuestionsAsync();
            return View(result);
        }

        // GET: Question/Details/5
        public async Task<ActionResult> Details(int? id)
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
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: Question/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection, Question question)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
                }

                var result = await questionRepo.Add(question);

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
                return View(question);
            }
        }

        // GET: Question/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }

            var result = await questionRepo.GetQuestionByIdAsync(id.Value);

            if (result == null)
            {
                ModelState.AddModelError("", "Not Found");
            }

            return View(result);
        }

        // POST: Question/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id, IFormCollection collection, Question question)
        {
            try
            {
                if (id == null)
                {
                    return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
                }

                var result = await questionRepo.Delete(question);

                if (result == null)
                {
                    return Redirect("/ErrorPage/404");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Delete not succeeded: " + ex.Message.ToString());
                return View(question);
            }
        }
    }
}