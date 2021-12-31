namespace AliceMafia.Application
{
    public class DialogStartState : DialogStateBase
    {
        public DialogStartState(IUserContext context) : base(context)
        {
        }

        public override void HandleUserRequest(AliceRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}