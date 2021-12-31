namespace AliceMafia.Application
{
    public class InGameState : DialogStateBase
    {
        public InGameState(IUserContext context) : base(context)
        {
        }

        public override AliceResponse HandleUserRequest(AliceRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}