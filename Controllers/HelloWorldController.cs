using Microsoft.AspNetCore.Mvc;

namespace AliceMafia.Controllers
{
    [ApiController]
    [Route("/")]
    public class HelloWorldController : ControllerBase
    {
        [HttpGet]
        public string HelloWord()
        {
            return "Hello, World!";
        }
    }
}