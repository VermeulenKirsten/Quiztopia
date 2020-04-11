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
    public class AnswerController : Controller
    {
        private readonly IAnswerRepo answerRepo;

        public AnswerController(IAnswerRepo answerRepo)
        {
            this.answerRepo = answerRepo;
        }

        // GET: Answer
        public async Task<ActionResult> Index()
        {
            var result = await answerRepo.GetAllAnswersAsync();
            return View(result);
        }

        // GET: Answer/Details/5
        public async Task<ActionResult> Details(int? id)
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
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: Answer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection, Answer answer)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
                }

                var result = await answerRepo.Add(answer);

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
                return View(answer);
            }
        }

        // GET: Answer/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }

            var result = await answerRepo.GetAnswerByIdAsync(id.Value);

            if (result == null)
            {
                ModelState.AddModelError("", "Not Found");
            }

            return View(result);
        }

        // POST: Answer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id, IFormCollection collection, Answer answer)
        {
            try
            {
                if (id == null)
                {
                    return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
                }

                var result = await answerRepo.Delete(answer);

                if (result == null)
                {
                    return Redirect("/ErrorPage/404");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Delete not succeeded: " + ex.Message.ToString());
                return View(answer);
            }
        }
    }
}