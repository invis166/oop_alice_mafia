using System.Collections.Concurrent;
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
        private static ConcurrentDictionary<string, UserContextBase> activeUsers = new ConcurrentDictionary<string, UserContextBase>();

        [HttpPost]
        public AliceResponse AlicePost(AliceRequest request, UserContextBase context)
        {
            var sessionId = request.Session.SessionId;
            if (!activeUsers.ContainsKey(sessionId))
            {
                activeUsers[sessionId] = context;
                context.ChangeState(new DialogStartState(context));
            }

            return activeUsers[sessionId].HandleUserRequest(request);
        }
        
        public static string CreateLobby(IGameSetting setting)
        {
            var game = CreateGame();
            var lobby = new GameLobby(game);
            game.SetSetting(setting);
            
            lobbies[lobby.Id] = lobby;

            return lobby.Id;
        }

        public static GameLobby GetLobbyById(string lobbyId)
        {
            return lobbies.ContainsKey(lobbyId) ? lobbies[lobbyId] : null;
        }
        
        public static IGame CreateGame()
        {
            return DIContainer.ConfigurateContainer().Get<IGame>();
        }
    }
}
