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
            // приходит запрос на создать лобби -> создаем лобби
            // приходит запрос на присоединиться к лобби -> присоединяемся к лобби
            // приходит запрос на запустить игру -> запускаем игру
            return new AliceResponse {Response = new ResponseModel { Text = "Привет, мир" } };
        }
    }
}