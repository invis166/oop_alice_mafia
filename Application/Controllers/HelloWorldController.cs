using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using AliceMafia.Application;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AliceMafia.Controllers
{
    [ApiController]
    [Route("/")]
    public class HelloWorldController : ControllerBase
    {
        private ConcurrentDictionary<string, GameLobby> lobbies = new ConcurrentDictionary<string, GameLobby>();
        
        [HttpGet]
        public string HelloWord()
        {
            return "Hello, World!";
        }

        // public HelloWorldController(IUserService service)
        // {
        //     this.service = service;
        // }
        
        public AliceResponse AlicePost(AliceRequest request)
        { 
            // приходит запрос на создать лобби -> создаем лобби
            // приходит запрос на присоединиться к лобби -> присоединяемся к лобби
            // приходит запрос на запустить игру -> запускаем игру
            var command = request.Request.Command;
            if (command.Contains("создать"))
            {
                var lobby = new GameLobby();
                lobbies[lobby.Id] = lobby;
                lobby.AddPlayer(request.Session.UserId);
            }
            return new AliceResponse {Response = new ResponseModel { Text = "", Buttons = new List<ButtonModel>
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