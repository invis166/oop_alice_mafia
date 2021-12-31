using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AliceMafia.Application;
using AliceMafia.Infrastructure;
using AliceMafia.Setting;
using Microsoft.AspNetCore.Mvc;
using Ninject;
using Ninject.Parameters;

namespace AliceMafia.Controllers
{
    [ApiController]
    [Route("/")]
    public class AliceMafiaController : ControllerBase
    {
        private static ConcurrentDictionary<string, GameLobby> lobbies = new ConcurrentDictionary<string, GameLobby>();
        

        private Dictionary<IGameSetting, string> FillSettings()
        {
            var settingsConstructorInfos = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterface("IGameSetting") != null && t.Namespace.Contains("AliceMafia.Setting"))
                .Select(t  => t.GetConstructor(Type.EmptyTypes))
                .ToList();
            

            var result = new Dictionary<IGameSetting, string>();
            foreach (var settingsConstructor in settingsConstructorInfos)
            {
                var setting = (IGameSetting)settingsConstructor.Invoke(new object[0]);
                result[setting] = setting.SettingName;
            }

            return result;
        }

        private Dictionary<string, IGameSetting> FillInverseSettings()
        {
            var settingsConstructorInfos = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterface("IGameSetting") != null && t.Namespace.Contains("AliceMafia.Setting"))
                .Select(t  => t.GetConstructor(Type.EmptyTypes))
                .ToList();
            

            var result = new Dictionary<string, IGameSetting>();
            foreach (var settingsConstructor in settingsConstructorInfos)
            {
                var setting = (IGameSetting)settingsConstructor.Invoke(new object[0]);
                result[setting.SettingName.ToLower()] = setting;
            }

            return result;
        }

        private AliceResponse CreateResponse(DialogState dialogState, List<ButtonModel> buttons = null,
            string name = "", string responseText = "", string gameId = null)
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
                    DialogState = dialogState,
                    Name = name,
                    GameId = gameId
                }
            };
        }

        private List<ButtonModel> CreateButtonList(params string[] buttonTexts) =>
            buttonTexts.Select(buttonText => new ButtonModel {Title = buttonText}).ToList();


        [HttpPost]
        public AliceResponse AlicePost(AliceRequest request)
        {
            var gameId = request.State.Session.GameId;

            switch (request.State.Session.DialogState)
            {
                case DialogState.DialogStart:
                    return CreateResponse(DialogState.WriteName,
                        responseText: "Привет! В этом навыке вы сможете сыграть в Мафию. Как вас зовут?");

                case DialogState.WriteName:
                    return CreateResponse(DialogState.JoinGame,
                        CreateButtonList("Создать комнату", "Присоединиться к игре"),
                        request.Request.Command,
                        "Отлично! Теперь можно играть. Выберите, что вы хотите сделать.");

                case DialogState.SettingSelection:
                    return ProcessSettingselection(request);
                
                case DialogState.JoinGame:
                    return ProcessLobbyStarting(request);
                
                case DialogState.CreateLobby:
                    return CreateLobby(request);

                case DialogState.StartGame:
                    return ProcessMafiaStarting(request, gameId);

                case DialogState.WriteLobby when !lobbies.ContainsKey(request.Request.Command):
                    return CreateResponse(DialogState.WriteLobby, name: request.State.Session.Name,
                        responseText: "Такой игры нет:( Попробуйте снова! Введите номер комнаты:");

                case DialogState.WriteLobby when lobbies[request.Request.Command].GameStarted:
                    return CreateResponse(DialogState.JoinGame,
                        CreateButtonList("Создать комнату", "Присоединиться к игре"),
                        responseText:
                        "Игра уже начата. К сожалению, так выпала карта." +
                        " Попробуйте начать свою игру или присоединиться к другой.",
                        name: request.State.Session.Name);

                case DialogState.WriteLobby:
                    lobbies[request.Request.Command].AddPlayer(request.Session.SessionId, request.State.Session.Name);
                    return CreateResponse(DialogState.Wait, CreateButtonList("Начать игру!"),
                        responseText: "Вы успешно присоединились к игре. Ожидайте начала!",
                        gameId: request.Request.Command);

                case DialogState.Wait when !request.Request.Command.Contains("начать игру"):
                    return CreateResponse(DialogState.Wait, CreateButtonList("Начать игру!"),
                        responseText: "Я бы хотела понять вас, но я всего лишь студенческий проект. Ожидайте начала!",
                        gameId: gameId);

                case DialogState.Wait when lobbies[request.State.Session.GameId].GameStarted:
                    return CreateResponse(DialogState.InGame, CreateButtonList("Далее"), responseText: "Игра началась!", gameId: gameId);

                case DialogState.Wait:
                    return CreateResponse(DialogState.Wait, CreateButtonList("Начать игру!"),
                        responseText: "Игра еще не началась, подождите.",
                        gameId: gameId);

                case DialogState.InGame:
                    return lobbies[request.State.Session.GameId].HandleRequest(request);

                default:
                    return CreateResponse(DialogState.EasterEgg,
                        responseText:
                        "[Пасхальная система пасхалок]\nПоздравляем!\nВы нашли пасхалку!\n" +
                        "Пожалуйста напишите в тг разработчиков, чтобы получить свой приз\n" +
                        "@xoposhiy");
            }
        }

        private AliceResponse ProcessSettingselection(AliceRequest request)
        {
            var settings = FillSettings();
            var buttons = CreateButtonList(settings.Keys.Select(key => settings[key]).ToArray());
            
            return CreateResponse(DialogState.CreateLobby, buttons, request.State.Session.Name,
                "Пожалуйста, выберите сеттинг для лобби");
        }

        private AliceResponse ProcessLobbyStarting(AliceRequest request)
        {
            var todo = request.Request.Command;
            if (todo.Contains("создать комнату"))
                return ProcessSettingselection(request);

            if (todo.Contains("присоединиться к игре"))
                return CreateResponse(DialogState.WriteLobby, name: request.State.Session.Name,
                    responseText: "Введите номер комнаты:");

            return CreateResponse(DialogState.JoinGame,
                CreateButtonList("Создать комнату", "Присоединиться к игре"),
                request.Request.Command,
                "Очень содержательно, но я вас не поняла. Выберите, что вы хотите сделать.");
        }

        private AliceResponse ProcessMafiaStarting(AliceRequest request, string gameId)
        {
            if (!request.Request.Command.Contains("начать игру"))
                return HandleInvalidText(gameId);

            var players = lobbies[gameId].PlayersCount;

            if (players < 3)
            {
                return CreateResponse(DialogState.StartGame, CreateButtonList("Начать игру!"),
                    responseText: $"Для игры нужно минимум трое. Пока что присоединилось всего {players}.",
                    gameId: gameId);
            }

            lobbies[gameId].StartGame();

            return CreateResponse(DialogState.InGame, CreateButtonList("Далее"), responseText: "Игра началась!", gameId: gameId);
        }

        private AliceResponse HandleInvalidText(string gameId)
        {
            return CreateResponse(DialogState.StartGame, CreateButtonList("Начать игру!"),
                responseText:
                $"Мне жаль, я не говорю на испанском. Номер комнаты: {gameId}." +
                " Когда все игроки присоединятся, нажмите \"Начать игру!\".",
                gameId: gameId);
        }

        private AliceResponse CreateLobby(AliceRequest request)
        {
            var inverseSettings = FillInverseSettings();
            var todo = request.Request.Command;
            var neededSetting = inverseSettings[todo];
            
            var kernel = new StandardKernel(new ServiceModule());
            var lobby = new GameLobby(kernel.Get<IGame>(new ConstructorArgument("gameSetting", neededSetting)));
            
            lobbies[lobby.Id] = lobby;
            lobby.AddPlayer(request.Session.SessionId,
                request.State.Session.Name);

            return CreateResponse(DialogState.StartGame, CreateButtonList("Начать игру!"),
                responseText:
                $"Номер комнаты: {lobby.Id}. Когда все игроки присоединятся, нажмите \"Начать игру!\".",
                gameId: lobby.Id);
        }
    }
}