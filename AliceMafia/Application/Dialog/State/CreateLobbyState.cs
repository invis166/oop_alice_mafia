using AliceMafia.Application.Dialog;
using AliceMafia.Infrastructure;
using Ninject;
using Ninject.Activation;
using Ninject.Parameters;

namespace AliceMafia.Application
{
    public class CreateLobbyState : DialogStateBase
    {
        public CreateLobbyState(UserContextBase context) : base(context)
        {
        }

        public override AliceResponse HandleUserRequest(AliceRequest request)
        {
            var inverseSettings = Utils.FillInverseSettings();
            var todo = request.Request.Command;
            var neededSetting = inverseSettings[todo];
            var lobbyId = context.CreateLobby(neededSetting);
            var lobby = context.GetLobbyById(lobbyId);
            
            lobby.AddPlayer(request.Session.SessionId, context.PlayerName);
            context.ChangeState(new HostStartGameState(context));
            context.LobbyId = lobbyId;

            return Utils.CreateResponse(
                $"Номер комнаты: {lobbyId}. Когда все игроки присоединятся, нажмите \"Начать игру!\".",
                Utils.CreateButtonList("Начать игру!"));
        }
    }
}