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
    public class TopicController : Controller
    {
        private readonly ITopicRepo topicRepo;

        public TopicController(ITopicRepo topicRepo)
        {
            this.topicRepo = topicRepo;
        }

        // GET: Topic
        public async Task<ActionResult> Index()
        {
            var result = await topicRepo.GetAllTopicsAsync();
            return View(result);
        }

        // GET: Topic/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }

            var result = await topicRepo.GetTopicByIdAsync(id.Value);

            if (result == null)
            {
                return Redirect("/ErrorPage/404");
            }
            return View(result);
        }

        // GET: Topic/Create
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: Topic/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection, Topic topic)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
                }

                var result = await topicRepo.Add(topic);

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
                return View(topic);
            }
        }

        // GET: Topic/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }

            var result = await topicRepo.GetTopicByIdAsync(id.Value);

            if (result == null)
            {
                return Redirect("/ErrorPage/404");
            }

            return View(result);
        }

        // POST: Topic/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, IFormCollection collection, Topic topic)
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

                var result = await topicRepo.Update(topic);

                if (result == null)
                {
                    return Redirect("/ErrorPage/404");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Update not succeeded: " + ex.Message.ToString());
                return View(topic);
            }
        }

        // GET: Topic/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }

            var result = await topicRepo.GetTopicByIdAsync(id.Value);

            if (result == null)
            {
                ModelState.AddModelError("", "Not Found");
            }

            return View(result);
        }

        // POST: Topic/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id, IFormCollection collection, Topic topic)
        {
            try
            {
                if (id == null)
                {
                    return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
                }

                var result = await topicRepo.Delete(topic);

                if (result == null)
                {
                    return Redirect("/ErrorPage/404");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Delete not succeeded: " + ex.Message.ToString());
                return View(topic);
            }
        }
    }
}