using AliceMafia.Application.Dialog;

namespace AliceMafia.Application
{
    public class EnterNameState : DialogStateBase
    {
        public EnterNameState(UserContextBase context) : base(context)
        {
        }

        public override AliceResponse HandleUserRequest(AliceRequest request)
        {
            context.ChangeState(new JoinGameState(context));
            context.PlayerName = request.Request.Command;
            return Utils.CreateResponse(
                "Отлично! Теперь можно играть. Выберите, что вы хотите сделать.",
                Utils.CreateButtonList("Создать комнату", "Присоединиться к игре"));
        }
    }
}