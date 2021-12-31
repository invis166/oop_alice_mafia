namespace AliceMafia.Application
{
    public class InGameState : DialogStateBase
    {
        public InGameState(IUserContext context) : base(context)
        {
        }

        public override void HandleUserRequest(AliceRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}