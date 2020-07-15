using System;
using Kwis.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kwis.Controllers
{
    [Route("api/[controller]")]
    public class SystemController : Controller
    {
        private readonly IQuizService quizService;

        public SystemController(IQuizService quizService)
        {
            this.quizService = quizService;
        }

        // Call an initialization - api/system/init
        [HttpGet("{setting}")]
        public string Get(string setting)
        {
            if (setting == "init")
            {
                //TODO: Setup the database

                return "Database was created, and collections 'Quiz' were created";
            }

            return "Unknown";
        }
    }
}
