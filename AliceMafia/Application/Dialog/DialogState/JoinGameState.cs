using AliceMafia.Application.Dialog;

namespace AliceMafia.Application
{
    public class JoinGameState : DialogStateBase
    {
        public JoinGameState(UserContextBase context) : base(context)
        {
        }

        public override AliceResponse HandleUserRequest(AliceRequest request)
        {
            var todo = request.Request.Command;
            if (todo.Contains("создать комнату"))
                return new SelectSettingState(context).HandleUserRequest(request);

            if (todo.Contains("присоединиться к игре"))
            {
                context.ChangeState(new EnterLobbyIdState(context));
                return Utils.CreateResponse("Введите номер комнаты:");
            }

            return Utils.CreateResponse(
                "Очень содержательно, но я вас не поняла. Выберите, что вы хотите сделать.",
                Utils.CreateButtonList("Создать комнату", "Присоединиться к игре"));
        }
    }
}