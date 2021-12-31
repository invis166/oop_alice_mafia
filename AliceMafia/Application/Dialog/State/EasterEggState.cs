namespace AliceMafia.Application
{
    public class EasterEggState : DialogStateBase
    {
        public EasterEggState(IUserContext context) : base(context)
        {
        }

        public override AliceResponse HandleUserRequest(AliceRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}