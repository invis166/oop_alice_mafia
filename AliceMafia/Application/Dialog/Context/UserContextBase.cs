using AliceMafia.Controllers;
using AliceMafia.Setting;

namespace AliceMafia.Application
{
    public abstract class UserContextBase
    {
        public string PlayerName { get; set; }
        public string LobbyId { get; set; }
        protected ControllerData data;

        public UserContextBase(ControllerData data)
        {
            this.data = data;
        }
        
        public abstract void ChangeState(DialogStateBase state);
        public abstract string CreateLobby(IGameSetting setting);
        public abstract GameLobby GetLobbyById(string lobbyId);
    }
}