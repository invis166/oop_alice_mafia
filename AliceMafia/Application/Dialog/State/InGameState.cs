namespace AliceMafia.Application
{
    public class InGameState : DialogStateBase
    {
        public InGameState(UserContextBase context) : base(context)
        {
        }

        public override AliceResponse HandleUserRequest(AliceRequest request)
        {
            var lobbyId = request.Request.Command;
            var lobby = context.GetLobbyById(lobbyId);

            return lobby.HandleRequest(request);
        }
    }
}