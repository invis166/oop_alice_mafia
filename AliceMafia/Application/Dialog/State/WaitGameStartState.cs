namespace AliceMafia.Application
{
    public class WaitGameStartState : DialogStateBase
    {
        public WaitGameStartState(IUserContext context) : base(context)
        {
        }

        public override AliceResponse HandleUserRequest(AliceRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}