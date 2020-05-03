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
    public class DifficultyController : Controller
    {
        private readonly IDifficultyRepo difficultyRepo;

        public DifficultyController(IDifficultyRepo difficultyRepo)
        {
            this.difficultyRepo = difficultyRepo;
        }

        // GET: Difficulty
        public async Task<ActionResult> Index()
        {
            var result = await difficultyRepo.GetAllDifficultiesAsync();
            return View(result);
        }

        // GET: Difficulty/Create
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: Difficulty/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection, Difficulty difficulty)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Redirect("/ErrorPage/400");
                }

                var result = await difficultyRepo.Add(difficulty);

                if (result == null)
                {
                    return Redirect("/ErrorPage/404");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Create is giving an error: " + ex.Message);
                ModelState.AddModelError("", "Create action failed: " + ex.Message);
                return View(difficulty);
            }
        }

        // GET: Difficulty/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("/ErrorPage/400");
            }

            var result = await difficultyRepo.GetDifficultyByIdAsync(id.Value);

            if (result == null)
            {
                return Redirect("/ErrorPage/404");
            }

            return View(result);
        }

        // POST: Difficulty/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, IFormCollection collection, Difficulty difficulty)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Validation Error");
                }

                if (id == null)
                {
                    return Redirect("/ErrorPage/400");
                }

                var result = await difficultyRepo.Update(difficulty);

                if (result == null)
                {
                    return Redirect("/ErrorPage/404");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Update not succeeded: " + ex.Message.ToString());
                return View(difficulty);
            }
        }

        // GET: Difficulty/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("/ErrorPage/400");
            }

            var result = await difficultyRepo.GetDifficultyByIdAsync(id.Value);

            if (result == null)
            {
                Redirect("/ErrorPage/404");
            }

            return View(result);
        }

        // POST: Difficulty/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id, IFormCollection collection, Difficulty difficulty)
        {
            try
            {
                if (id == null)
                {
                    return Redirect("/ErrorPage/400");
                }

                var result = await difficultyRepo.Delete(difficulty);

                if (result == null)
                {
                    return Redirect("/ErrorPage/404");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Delete not succeeded: " + ex.Message.ToString());
                return View(difficulty);
            }
        }
    }
}