using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        [HttpPost]
        public AliceResponse AlicePost(AliceRequest request)
        {
            return new AliceResponse {Response = new ResponseModel { Text = "Привет, мир" } };
        }

    }
}