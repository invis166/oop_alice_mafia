using System;
using System.Collections.Generic;
using AliceMafia.Application;
using AliceMafia.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AliceMafia.Controllers
{
    [ApiController]
    [Route("/")]
    public class HelloWorldController : ControllerBase
    {
        private IUserService service;
        
        [HttpGet]
        public string HelloWord()
        {
            return "Hello, World!";
        }

        // public HelloWorldController(IUserService service)
        // {
        //     this.service = service;
        // }

        [HttpPost]
        public AliceResponse AlicePost([FromBody] dynamic request)
        { 
            // приходит запрос на создать лобби -> создаем лобби
            // приходит запрос на присоединиться к лобби -> присоединяемся к лобби
            // приходит запрос на запустить игру -> запускаем игру
            
            var s = "1";
            var r = Request;
            return new AliceResponse {Response = new ResponseModel { Text = s, Buttons = new List<ButtonModel>
            {
                new ButtonModel{Title = "Нажми!!", Hide = true},
                new ButtonModel{Title = "Нажми пж..."}
            } } };
            // if (request.State.Session.GameId == string.Empty)
            // {
            //     request.
            // }
            // else
            // {
            //     service.Database.GetLobby(request.State.Session.GameId).HandleRequest(request);
            // }
        }
    }
}