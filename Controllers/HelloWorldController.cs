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
        public string AlicePost(AliceRequest request)
        {
            return JsonConvert.SerializeObject(request);
        }

    }
}