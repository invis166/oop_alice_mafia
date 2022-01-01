using System.Data.Common;
using AliceMafia.Application.Dialog;

namespace AliceMafia.Application
{
    public class EnterLobbyIdState : DialogStateBase
    {
        public EnterLobbyIdState(UserContextBase context) : base(context)
        {
        }

        public override AliceResponse HandleUserRequest(AliceRequest request)
        {
            var lobbyId = request.Request.Command;
            var lobby = context.GetLobbyById(lobbyId);
            var userId = request.Session.SessionId;
            if (lobby is null)
            {
                context.ChangeState(new EnterLobbyIdState(context));
                
                return Utils.CreateResponse("Такой игры нет:( Попробуйте снова! Введите номер комнаты:");
            }

            if (lobby.GameStarted)
            {
                context.ChangeState(new JoinGameState(context));
                return Utils.CreateResponse(
                    "Игра уже начата. К сожалению, так выпала карта." +
                    " Попробуйте начать свою игру или присоединиться к другой.",
                    Utils.CreateButtonList("Создать комнату", "Присоединиться к игре"));
            }
            
            context.LobbyId = lobbyId;
            context.ChangeState(new WaitGameStartState(context));
            lobby.AddPlayer(userId, context.PlayerName);
            return Utils.CreateResponse(
                "Вы успешно присоединились к игре. Ожидайте начала!",
                Utils.CreateButtonList("Начать игру!"));
        }
    }
}