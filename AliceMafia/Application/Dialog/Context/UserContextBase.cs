using AliceMafia.Controllers;
using AliceMafia.Setting;

namespace AliceMafia.Application
{
    public abstract class UserContextBase
    {
        public string PlayerName { get; set; }
        public string LobbyId { get; set; }
        protected ControllerData data;
        protected DialogStateBase state;

        public UserContextBase(ControllerData data)
        {
            this.data = data;
        }

        public AliceResponse HandleUserRequest(AliceRequest request)
        {
            return state.HandleUserRequest(request);
        }
        
        public abstract void ChangeState(DialogStateBase state);
        public abstract string CreateLobby(IGameSetting setting);
        public abstract GameLobby GetLobbyById(string lobbyId);
    }
}