using AliceMafia.Controllers;

namespace AliceMafia.Application
{
    public class InGameState : DialogStateBase
    {
        public InGameState(UserContextBase context) : base(context)
        {
        }

        public override AliceResponse HandleUserRequest(AliceRequest request)
        {
            var lobby = AliceMafiaController.GetLobbyById(context.LobbyId);

            return lobby.HandleRequest(request);
        }
    }
}