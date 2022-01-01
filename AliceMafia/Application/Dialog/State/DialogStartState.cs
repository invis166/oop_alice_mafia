using AliceMafia.Application.Dialog;

namespace AliceMafia.Application
{
    public class DialogStartState : DialogStateBase
    {
        public DialogStartState(UserContextBase context) : base(context)
        {
        }

        public override AliceResponse HandleUserRequest(AliceRequest request)
        {
            context.ChangeState(new EnterNameState(context));
            
            return Utils.CreateResponse(responseText: "Привет! В этом навыке вы сможете сыграть в Мафию. Как вас зовут?");
        }
    }
}