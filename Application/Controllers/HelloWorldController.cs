using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using AliceMafia.Application;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AliceMafia.Controllers
{
    [ApiController]
    [Route("/")]
    public class HelloWorldController : ControllerBase
    {
        private static ConcurrentDictionary<string, GameLobby> lobbies = new ConcurrentDictionary<string, GameLobby>();
        
        [HttpGet]
        public string HelloWord()
        {
            return "Hello, World!";
        }

        // public HelloWorldController(IUserService service)
        // {
        //     this.service = service;
        // }

        private AliceResponse CreateResponse(DialogState dialogState, List<ButtonModel> buttons = null, 
            string name = "", string responseText ="", string gameId = null)
        {
            return new AliceResponse
            {
                Response = new ResponseModel
                {
                    Text = responseText,
                    Buttons = buttons,
                },
                State = new StateModel
                {
                    DgState = dialogState,
                    Name = name,
                    GameId = gameId
                }
            };
        }

        private List<ButtonModel> CreateButtonList(List<string> buttonTexts) =>
            buttonTexts.Select(buttonText => new ButtonModel {Title = buttonText}).ToList();


        public AliceResponse AlicePost(AliceRequest request)
        {
            var gameId = request.State.Session.GameId;
            
            switch (request.State.Session.DgState)
            {
                case DialogState.StartState:
                    return CreateResponse(DialogState.WriteName,
                        responseText: "Привет! В этом навыке вы сможете сыграть в Мафию. Как вас зовут?");
                
                case DialogState.WriteName:
                {
                    var buttons = new List<ButtonModel>
                    {
                        new ButtonModel {Title = "Создать комнату"},
                        new ButtonModel {Title = "Присоединиться к игре"}
                    };
                    
                    return CreateResponse(DialogState.GameStartState, buttons, request.Request.Command,
                        "Отлично! Теперь можно играть. Выберите, что вы хотите сделать.");
                }
                
                case DialogState.GameStartState:
                {
                    var todo = request.Request.Command;
                    var buttons = new List<ButtonModel>();
                    if (todo.Contains("создать комнату"))
                    {
                        var lobby = new GameLobby();
                        lobbies[lobby.Id] = lobby;
                        lobby.AddPlayer(request.Session.UserId, request.State.Session.Name);
                    
                        buttons.Add(new ButtonModel { Title = "Начать игру!" } );
                        return CreateResponse(DialogState.StartMafia, buttons,
                            responseText:
                            $"Номер комнаты: {lobby.Id}. Когда все игроки присоединятся, нажмите \"Начать игру!\".",
                            gameId: lobby.Id);
                    }

                    if (todo.Contains("присоединиться к игре"))
                    {
                        return CreateResponse(DialogState.WriteLobby, name:request.State.Session.Name, 
                            responseText:"Введите номер комнаты:");
                    }

                    buttons.Concat( new List<ButtonModel>
                    {
                        new ButtonModel {Title = "Создать комнату"},
                        new ButtonModel {Title = "Присоединиться к игре"}
                    });
                    return CreateResponse(DialogState.GameStartState, buttons, request.Request.Command,
                        "Очень содержательно, но я вас не поняла. Выберите, что вы хотите сделать.");
                }
                
                case DialogState.StartMafia:
                {
                    var buttons = new List<ButtonModel>();
                    if (!request.Request.Command.Contains("начать игру"))
                    {
                        buttons.Add(new ButtonModel {Title = "Начать игру!"});
                        return CreateResponse(DialogState.StartMafia, buttons,
                            responseText:
                            $"Мне жаль, я не говорю на испанском. Номер комнаты: {gameId}." +
                            " Когда все игроки присоединятся, нажмите \"Начать игру!\".",
                            gameId: gameId);
                    }
                
                    var players = lobbies[gameId].PlayersCount;
                    buttons.Add(new ButtonModel {Title = "Начать игру!"});
                    if (players < 3)
                    {
                        return CreateResponse(DialogState.StartMafia, buttons,
                            responseText: $"Для игры нужно минимум трое. Пока что присоединилось всего {players}.",
                            gameId: gameId);
                    }
                
                    lobbies[gameId].StartGame();

                    return CreateResponse(DialogState.InGame, responseText: "Игра началась!", gameId: gameId);
                }
                
                case DialogState.WriteLobby when !lobbies.ContainsKey(request.Request.Command):
                    return CreateResponse(DialogState.WriteLobby, name: request.State.Session.Name,
                        responseText: "Такой игры нет:( Попробуйте снова! Введите номер комнаты:");
                
                case DialogState.WriteLobby when lobbies[request.Request.Command].GameStarted:
                    return CreateResponse(DialogState.GameStartState,
                        CreateButtonList(new List<string> {"Создать комнату", "Присоединиться к игре"}),
                        responseText:
                        "Игра уже начата. К сожалению, так выпала карта." +
                        " Попробуйте начать свою игру или присоединиться к другой.",
                        name: request.State.Session.Name);
                
                case DialogState.WriteLobby:
                    lobbies[request.Request.Command].AddPlayer(request.Session.UserId, request.State.Session.Name);

                    return CreateResponse(DialogState.Wait, CreateButtonList(new List<string> {"Начать игру!"}),
                        responseText: "Вы успешно присоединились к игре. Ожидайте начала!",
                        gameId: request.Request.Command);
                
                case DialogState.Wait when !request.Request.Command.Contains("начать игру"):
                    return CreateResponse(DialogState.Wait, CreateButtonList(new List<string> {"Начать игру!"}),
                        responseText: "Я бы хотела понять вас, но я всего лишь студенческий проект. Ожидайте начала!",
                        gameId: gameId);
                
                case DialogState.Wait when lobbies[request.State.Session.GameId].GameStarted:
                    return CreateResponse(DialogState.InGame, responseText: "Игра началась!", gameId: gameId);
                
                case DialogState.Wait:
                    return CreateResponse(DialogState.Wait, CreateButtonList(new List<string> {"Начать игру!"}),
                        responseText: "Игра еще не началась, подождите.",
                        gameId: gameId);
                
                case DialogState.InGame:
                    return lobbies[request.State.Session.GameId].HandleRequest(request);
                
                default:
                    return new AliceResponse
                    {
                        Response = new ResponseModel
                        {
                            Text = "[Пасхальная система пасхалок]\nПоздравляем!\nВы нашли пасхалку!\n" +
                                   "Пожалуйста обратитесь на почту разработчиков, чтобы получить свой приз\n" +
                                   "kolesnikovv028@gmail.com"
                        },
                    };
            }
        }
    }
}