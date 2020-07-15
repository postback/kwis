using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Kwis.Models;
using Kwis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Kwis.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    public class QuizController : ApiControllerBase
    {
        private readonly ILogger<QuizController> logger;
        private readonly IQuizService quizService;

        public QuizController(ILogger<QuizController> logger, IQuizService quizService)
        {
            this.logger = logger;
            this.quizService = quizService;
        }

        // GET api/quiz
        [HttpGet]
        public async Task<IEnumerable<Quiz>> Get()
        {
            return await quizService.Get();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Quiz>> GetById(string id)
        {
            var quiz = await quizService.Get(id);

            if (quiz == null)
            {
                return NotFound();
            }

            return quiz;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Update(string id, Quiz quiz)
        {
            if (id != quiz.Id)
            {
                return BadRequest();
            }

            var quizFound = await quizService.Get(id);
            if (quizFound == null)
            {
                return NotFound();
            }

            try
            {
                return await quizService.Update(id, quiz);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Quiz>> Create(Quiz quiz)
        {
            await quizService.Create(quiz);

            return CreatedAtAction(
                nameof(GetById),
                new { id = quiz.Id },
                quiz);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            var quiz = await quizService.Get(id);

            if (quiz == null)
            {
                return NotFound();
            }

            var result = await quizService.Remove(quiz);

            return result;
        }

        /*
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }*/
    }
}
