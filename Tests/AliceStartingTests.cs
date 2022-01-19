using System.Collections.Concurrent;
using System.Reflection;
using NUnit.Framework;
using AliceMafia.Application;
using AliceMafia.Controllers;

namespace AliceMafia
{
    public class AliceStartingTests
    {
        [TestCase]
        public void TestStarting()
        {
            var controller = new AliceMafiaController();
            controller.AlicePost(new AliceRequest {Session = new SessionModel{SessionId = "1"}});
            var activePlayers = GetActivePlayersDictionary(controller);
            var curState = GetPlayerContext(activePlayers["1"]);
            
            Assert.True(curState is EnterNameState);
        }
        
        [TestCase]
        public void TestWriteName()
        {
            var controller = new AliceMafiaController();
            var session = new SessionModel {SessionId = "2"};
            controller.AlicePost(new AliceRequest {Session = session});
            
            controller.AlicePost(new AliceRequest {Session = session, Request = new RequestModel{Command = "Валера"}});
            var activePlayers = GetActivePlayersDictionary(controller);
            var curState = GetPlayerContext(activePlayers["2"]);
            
            Assert.True(curState is JoinGameState);
        }
        
        [TestCase]
        public void TestCreateAndHostLobby()
        {
            var controller = new AliceMafiaController();
            var session = new SessionModel {SessionId = "3"};
            var request = new RequestModel {Command = "Вадик"};
            controller.AlicePost(new AliceRequest {Session = session});
            controller.AlicePost(new AliceRequest {Session = session, Request = request});
            
            request = new RequestModel {Command = "случайный текст"};
            controller.AlicePost(new AliceRequest {Session = session, Request = request});
            var activePlayers = GetActivePlayersDictionary(controller);
            var curState = GetPlayerContext(activePlayers["3"]);
            Assert.True(curState is JoinGameState);
            
            request = new RequestModel {Command = "создать комнату"};
            controller.AlicePost(new AliceRequest {Session = session, Request = request});
            activePlayers = GetActivePlayersDictionary(controller);
            curState = GetPlayerContext(activePlayers["3"]);
            Assert.True(curState is CreateLobbyState);
            
            request = new RequestModel {Command = "стандартный сеттинг"};
            controller.AlicePost(new AliceRequest {Session = session, Request = request});
            activePlayers = GetActivePlayersDictionary(controller);
            curState = GetPlayerContext(activePlayers["3"]);
            Assert.True(curState is HostStartGameState);
        }
        
        [TestCase]
        public void TestConnectToLobby()
        {
            var controller = new AliceMafiaController();
            var session = new SessionModel {SessionId = "4"};
            controller.AlicePost(new AliceRequest {Session = session});
            controller.AlicePost(new AliceRequest {Session = session, Request = new RequestModel {Command = "Федорик"}});
            controller.AlicePost(new AliceRequest {Session = session, Request = new RequestModel {Command = "создать комнату"}});
            controller.AlicePost(new AliceRequest {Session = session, Request = new RequestModel {Command = "стандартный сеттинг"}});
            var activePlayers = GetActivePlayersDictionary(controller);
            var lobbyId = activePlayers["4"].LobbyId;
            
            session = new SessionModel {SessionId = "5"};
            controller.AlicePost(new AliceRequest {Session = session});
            controller.AlicePost(new AliceRequest {Session = session, Request = new RequestModel {Command = "Крутой Джо"}});
            controller.AlicePost(new AliceRequest {Session = session, Request = new RequestModel {Command = "присоединиться к игре"}});
            controller.AlicePost(new AliceRequest {Session = session, Request = new RequestModel {Command = lobbyId}});
            activePlayers = GetActivePlayersDictionary(controller);
            var curState = GetPlayerContext(activePlayers["5"]);
            Assert.True(curState is WaitGameStartState);
        }
        
        private static ConcurrentDictionary<string, UserContextBase> GetActivePlayersDictionary(AliceMafiaController controller)
        {
            var field = controller.GetType().GetField("activeUsers", BindingFlags.NonPublic | BindingFlags.Static);

            return (ConcurrentDictionary<string, UserContextBase>) field.GetValue(controller);
        }
        
        private static DialogStateBase GetPlayerContext(UserContextBase userContext)
        {
            var field = userContext.GetType().GetField("state", BindingFlags.NonPublic | BindingFlags.Instance);

            return (DialogStateBase) field.GetValue(userContext);
        }
    }
}