namespace AliceMafia.Application
{
    public class EasterEggState : DialogStateBase
    {
        public EasterEggState(UserContextBase context) : base(context)
        {
        }

        public override AliceResponse HandleUserRequest(AliceRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}