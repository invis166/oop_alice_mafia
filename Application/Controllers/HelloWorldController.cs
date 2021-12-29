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
        
        public AliceResponse AlicePost(AliceRequest request)
        { 
            if (request.State.Session.DgState == DialogState.StartState)
            {
                return new AliceResponse
                {
                    Response = new ResponseModel
                    {
                        Text = "Привет! В этом навыке вы сможете сыграть в Мафию. Как вас зовут?",
                    },
                    State = new StateModel
                    {
                        DgState = DialogState.WriteName
                    }
                };
            }

            if (request.State.Session.DgState == DialogState.WriteName)
            {
                return new AliceResponse
                {
                    Response = new ResponseModel
                    {
                        Text = "Отлично! Теперь можно играть. Выберите, что вы хотите сделать.",
                        Buttons = new List<ButtonModel>
                        {
                            new ButtonModel {Title = "Создать комнату"},
                            new ButtonModel {Title = "Присоединиться к игре"}
                        }
                    },
                    State = new StateModel
                    {
                        DgState = DialogState.GameStartState,
                        Name = request.Request.Command
                    }
                };
            }
            
            if (request.State.Session.DgState == DialogState.GameStartState)
            {
                var todo = request.Request.Command;
                if (todo.Contains("создать комнату"))
                {
                    var lobby = new GameLobby();
                    lobbies[lobby.Id] = lobby;
                    lobby.AddPlayer(request.Session.UserId, request.State.Session.Name);
                    return new AliceResponse {Response = new ResponseModel { Text = $"Номер комнаты: {lobby.Id}. Когда все игроки присоединятся, нажмите \"Начать игру!\".", 
                            Buttons = new List<ButtonModel>
                            {
                                new ButtonModel{Title = "Начать игру!"}
                            } },
                        State = new StateModel
                        {
                            GameId = lobby.Id,
                            DgState = DialogState.StartMafia
                        }
                    };
                }

                if (todo.Contains("присоединиться к игре"))
                {
                    return new AliceResponse
                    {
                        Response = new ResponseModel
                        {
                            Text = "Введите номер комнаты:",
                        },
                        State = new StateModel
                        {
                            DgState = DialogState.WriteLobby,
                            Name = request.State.Session.Name
                        }
                    };
                }
                return new AliceResponse
                {
                    Response = new ResponseModel
                    {
                        Text = "Очень содержательно, но я вас не поняла. Выберите, что вы хотите сделать.",
                        Buttons = new List<ButtonModel>
                        {
                            new ButtonModel {Title = "Создать комнату"},
                            new ButtonModel {Title = "Присоединиться к игре"}
                        }
                    },
                    State = new StateModel
                    {
                        DgState = DialogState.GameStartState,
                        Name = request.Request.Command
                    }
                };
            }

            if (request.State.Session.DgState == DialogState.StartMafia)
            {
                if (!request.Request.Command.Contains("начать игру"))
                {
                    return new AliceResponse {Response = new ResponseModel { Text = $"Мне жаль, я не говорю на испанском. Номер комнаты: {request.State.Session.GameId}. Когда все игроки присоединятся, нажмите \"Начать игру!\".", 
                            Buttons = new List<ButtonModel>
                            {
                                new ButtonModel{Title = "Начать игру!"}
                            } },
                        State = new StateModel
                        {
                            GameId = request.State.Session.GameId,
                            DgState = DialogState.StartMafia
                        }
                    };
                }
                var gameId = request.State.Session.GameId;
                var players = lobbies[gameId].PlayersCount;
                if (players < 3)
                {
                    return new AliceResponse {Response = new ResponseModel { Text = $"Для игры нужно минимум трое. Пока что присоединилось всего {players}.", 
                            Buttons = new List<ButtonModel>
                            {
                                new ButtonModel{Title = "Начать игру!"}
                            } },
                        State = new StateModel
                        {
                            GameId = request.State.Session.GameId,
                            DgState = DialogState.StartMafia
                        }
                    };
                }
                lobbies[gameId].StartGame();
                return new AliceResponse {Response = new ResponseModel { Text = "Игра началась!"},
                    State = new StateModel
                    {
                        GameId = request.State.Session.GameId,
                        DgState = DialogState.InGame
                    }
                };
            }

            if (request.State.Session.DgState == DialogState.WriteLobby)
            {
                if (lobbies.ContainsKey(request.Request.Command))
                {
                    if (!lobbies[request.Request.Command].GameStarted)
                    {
                        lobbies[request.Request.Command].AddPlayer(request.Session.UserId, request.State.Session.Name);
                        return new AliceResponse {Response = new ResponseModel { Text = "Вы успешно присоединились к игре. Ожидайте начала!", 
                                Buttons = new List<ButtonModel>
                                {
                                    new ButtonModel{Title = "Начать игру!"}
                                } },
                            State = new StateModel
                            {
                                GameId = request.Request.Command,
                                DgState = DialogState.Wait
                            }
                        };
                    }
                    return new AliceResponse
                    {
                        Response = new ResponseModel
                        {
                            Text = "Игра уже начата. К сожалению, так выпала карта. Попробуйте начать свою игру или присоединиться к другой.",
                            Buttons = new List<ButtonModel>
                            {
                                new ButtonModel {Title = "Создать комнату"},
                                new ButtonModel {Title = "Присоединиться к игре"}
                            }
                        },
                        State = new StateModel
                        {
                            DgState = DialogState.GameStartState,
                            Name = request.State.Session.Name
                        }
                    };
                }
                return new AliceResponse
                {
                    Response = new ResponseModel
                    {
                        Text = "Такой игры нет:( Попробуйте снова! Введите номер комнаты:",
                    },
                    State = new StateModel
                    {
                        DgState = DialogState.WriteLobby,
                        Name = request.State.Session.Name
                    }
                };
            }

            if (request.State.Session.DgState == DialogState.Wait)
            {
                if (!request.Request.Command.Contains("начать игру"))
                {
                    return new AliceResponse {Response = new ResponseModel { Text = "Я бы хотела понять вас, но я всего лишь студенческий проект. Ожидайте начала!", 
                            Buttons = new List<ButtonModel>
                            {
                                new ButtonModel{Title = "Начать игру!"}
                            } },
                        State = new StateModel
                        {
                            GameId = request.State.Session.GameId,
                            DgState = DialogState.Wait
                        }
                    };
                }
                if (lobbies[request.State.Session.GameId].GameStarted)
                {
                    return new AliceResponse {Response = new ResponseModel { Text = "Игра началась!"},
                        State = new StateModel
                        {
                            GameId = request.State.Session.GameId,
                            DgState = DialogState.InGame
                        }
                    };
                }
                return new AliceResponse {Response = new ResponseModel { Text = "Игра еще не началась, подождите.", 
                        Buttons = new List<ButtonModel>
                        {
                            new ButtonModel{Title = "Начать игру!"}
                        } },
                    State = new StateModel
                    {
                        GameId = request.State.Session.GameId,
                        DgState = DialogState.Wait
                    }
                };
            }

            if (request.State.Session.DgState == DialogState.InGame)
            {
                return lobbies[request.State.Session.GameId].HandleRequest(request);
            }

            return new AliceResponse();
        }
    }
}