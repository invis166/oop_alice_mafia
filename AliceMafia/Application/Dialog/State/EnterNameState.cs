namespace AliceMafia.Application
{
    public class EnterNameState : DialogStateBase
    {
        public EnterNameState(IUserContext context) : base(context)
        {
        }

        public override AliceResponse HandleUserRequest(AliceRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}