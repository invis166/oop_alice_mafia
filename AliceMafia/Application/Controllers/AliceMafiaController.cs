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
        public AliceResponse AlicePost(AliceRequest request)
        {
            var sessionId = request.Session.SessionId;
            if (!activeUsers.ContainsKey(sessionId))
            {
                var kernel = new StandardKernel(new ServiceModule());
                var context = kernel.Get<UserContextBase>();
                activeUsers[sessionId] = context;
                context.ChangeState(new DialogStartState(context));
            }

            return activeUsers[sessionId].HandleUserRequest(request);
        }
        
        public static string CreateLobby(IGameSetting setting)
        {
            var kernel = new StandardKernel(new ServiceModule());
            var lobby = new GameLobby(kernel.Get<IGame>(new ConstructorArgument("gameSetting", setting)));
            
            lobbies[lobby.Id] = lobby;

            return lobby.Id;
        }

        public static GameLobby GetLobbyById(string lobbyId)
        {
            return lobbies.ContainsKey(lobbyId) ? lobbies[lobbyId] : null;
        }
    }
}
