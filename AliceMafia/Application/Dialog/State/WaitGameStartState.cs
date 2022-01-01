using AliceMafia.Application.Dialog;

namespace AliceMafia.Application
{
    public class WaitGameStartState : DialogStateBase
    {
        public WaitGameStartState(UserContextBase context) : base(context)
        {
        }

        public override AliceResponse HandleUserRequest(AliceRequest request)
        {
            var command = request.Request.Command;
            if (!command.Contains("начать игру"))
            {
                context.ChangeState(new WaitGameStartState(context));
                return Utils.CreateResponse(
                    "Я бы хотела понять вас, но я всего лишь студенческий проект. Ожидайте начала!",
                    Utils.CreateButtonList("Начать игру!"));
            }
            if (context.GetLobbyById(context.LobbyId).GameStarted)
            {
                context.ChangeState(new InGameState(context));
                return Utils.CreateResponse("Игра началась!", Utils.CreateButtonList("Далее"));
            }

            return new AliceResponse();
        }
    }
}