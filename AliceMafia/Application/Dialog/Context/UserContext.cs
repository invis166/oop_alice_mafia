using AliceMafia.Controllers;
using AliceMafia.Infrastructure;
using AliceMafia.Setting;
using Ninject;
using Ninject.Parameters;

namespace AliceMafia.Application
{
    public class UserContext : UserContextBase
    {
        public string LobbyId { get; set; }
        public string PlayerName { get; set; }
        
        public UserContext(ControllerData data) : base(data)
        {
        }
        
        public override void ChangeState(DialogStateBase state)
        {
            this.state = state;
        }

        public override string CreateLobby(IGameSetting setting)
        {
            var kernel = new StandardKernel(new ServiceModule());
            var lobby = new GameLobby(kernel.Get<IGame>(new ConstructorArgument("gameSetting", setting)));
            
            data.lobbies[lobby.Id] = lobby;

            return lobby.Id;
        }

        public override GameLobby GetLobbyById(string lobbyId)
        {
            return data.lobbies.ContainsKey(lobbyId) ? data.lobbies[lobbyId] : null;
        }
    }
}