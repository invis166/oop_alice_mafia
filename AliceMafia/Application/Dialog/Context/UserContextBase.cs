using AliceMafia.Controllers;
using AliceMafia.Setting;

namespace AliceMafia.Application
{
    public abstract class UserContextBase
    {
        public string PlayerName { get; set; }
        public string LobbyId { get; set; }
        protected DialogStateBase state;

        public AliceResponse HandleUserRequest(AliceRequest request)
        {
            return state.HandleUserRequest(request);
        }
        
        public abstract void ChangeState(DialogStateBase state);
    }
}