using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        public QuizController(IQuizRepo quizRepo)
        {
            this.quizRepo = quizRepo;
        }

        // GET: api/Quiz
        [HttpGet]
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
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Quiz/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
