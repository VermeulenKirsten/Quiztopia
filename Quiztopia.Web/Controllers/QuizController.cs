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
    public class QuizController : Controller
    {
        private readonly IQuizRepo quizRepo;
        private readonly ITopicRepo topicRepo;
        private readonly IDifficultyRepo difficultyRepo;

        public QuizController(IQuizRepo quizRepo, ITopicRepo topicRepo, IDifficultyRepo difficultyRepo)
        {
            this.quizRepo = quizRepo;
            this.topicRepo = topicRepo;
            this.difficultyRepo = difficultyRepo;
        }

        // GET: Quiz
        public async Task<ActionResult> Index()
        {
            var result = await quizRepo.GetAllQuizzesAsync();
            return View(result);
        }

        // GET: Quiz/Details/5
        public async Task<ActionResult> Details(int? id)
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
            return View(result);
        }

        // GET: Quiz/Create
        public async Task<ActionResult> Create()
        {
            return View(new CreateUpdateQuizVM(new Quiz(), topicRepo, difficultyRepo));
        }

        // POST: Quiz/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int id, IFormCollection collection, CreateUpdateQuizVM vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
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
        public async Task<ActionResult> Edit(int? id)
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

            return View(new CreateUpdateQuizVM(result, topicRepo, difficultyRepo));
        }

        // POST: Quiz/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, IFormCollection collection, CreateUpdateQuizVM vm)
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

                vm.Quiz.Id = Convert.ToInt32(id);

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
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }

            var result = await quizRepo.GetQuizByIdAsync(id.Value);

            if (result == null)
            {
                ModelState.AddModelError("", "Not Found");
            }

            return View(result);
        }

        // POST: Quiz/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id, IFormCollection collection, Quiz quiz)
        {
            try
            {
                if (id == null)
                {
                    return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
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
    }
}