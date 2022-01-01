using AliceMafia.Application.Dialog;

namespace AliceMafia.Application
{
    public class HostStartGameState : DialogStateBase
    {
        public HostStartGameState(UserContextBase context) : base(context)
        {
        }

        public override AliceResponse HandleUserRequest(AliceRequest request)
        {
            if (!request.Request.Command.Contains("начать игру"))
            {
                context.ChangeState(new HostStartGameState(context));
                return Utils.CreateResponse(
                    $"Мне жаль, я не говорю на испанском. Номер комнаты: {context.LobbyId}." +
                    " Когда все игроки присоединятся, нажмите \"Начать игру!\".",
                    Utils.CreateButtonList("Начать игру!"));
                
            }

            var lobby = context.GetLobbyById(context.LobbyId);
            if (lobby.PlayersCount < 3)
            {
                context.ChangeState(new HostStartGameState(context));
                return Utils.CreateResponse(
                    $"Для игры нужно минимум трое. Пока что присоединилось всего {lobby.PlayersCount}.",
                    Utils.CreateButtonList("Начать игру!"));
            }

            lobby.StartGame();
            context.ChangeState(new InGameState(context));

            return Utils.CreateResponse("Игра началась!", Utils.CreateButtonList("Далее"));
        }
    }
}