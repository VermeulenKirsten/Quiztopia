using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quiztopia.API.DTOs;
using Quiztopia.Models;
using Quiztopia.Models.Repositories;

namespace Quiztopia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizRepo quizRepo;
        private readonly ITopicRepo topicRepo;
        private readonly IDifficultyRepo difficultyRepo;
        private const string AuthSchemes = CookieAuthenticationDefaults.AuthenticationScheme + ",Identity.Application";

        public QuizController(IQuizRepo quizRepo, ITopicRepo topicRepo, IDifficultyRepo difficultyRepo)
        {
            this.quizRepo = quizRepo;
            this.topicRepo = topicRepo;
            this.difficultyRepo = difficultyRepo;
        }

        // GET: api/Quiz
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            //1. Get models async
            var model = await quizRepo.GetAllQuizzesAsync();

            //2. Mapping naar DTO
            List<QuizDTO> model_DTO = new List<QuizDTO>();

            foreach (Quiz item in model)
            {
                var result = new QuizDTO();
                model_DTO.Add(QuizMapper.ConvertTo_DTO(item, ref result));
            }
            return Ok(model_DTO);
        }


        // GET: api/Quiz/5
        [HttpGet("{name?}")]
        [Authorize(AuthenticationSchemes = AuthSchemes, Roles = "Admin")]
        public async Task<IActionResult> Get([FromRoute] string name = null)
        {
            try
            {
                if (name != null)
                {
                    var model = await quizRepo.GetQuizzesByNameAsync(name);

                    //2. Mapping naar DTO
                    List<QuizDTO> model_DTO = new List<QuizDTO>();

                    foreach (Quiz item in model)
                    {
                        var result = new QuizDTO();
                        model_DTO.Add(QuizMapper.ConvertTo_DTO(item, ref result));
                    }
                    return Ok(model_DTO);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return BadRequest();
            }
        }

        // POST: api/Quiz
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] QuizDTO dto)
        {
            var confirmedModel = new Quiz(); //te returnen DTO
            try
            {
                //1. Validatie
                if (!ModelState.IsValid) 
                { 
                    return BadRequest(ModelState); 
                
                }
                //2.Entity (model) via de mapper ophalen
                var model = new Quiz() { };

                model = await QuizMapper.ConvertTo_Entity(dto, topicRepo, difficultyRepo, model);

                //3. Entity (model) toevoegen via het repo
                confirmedModel = await quizRepo.Add(model);

                //4. Een bevestiging v foutieve actie teruggeven
                if (confirmedModel == null)
                {
                    return NotFound(model.Name + " was not saved");
                }
            }
            catch (Exception exc)
            {
                return BadRequest("Failed to add");
            }
            //5. DTO terugsturen als bevestiging
            return CreatedAtAction("Get", new { id = confirmedModel.Id }, dto);
        }
    }
}
