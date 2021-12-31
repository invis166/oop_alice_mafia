namespace AliceMafia.Application
{
    public class EasterEggState : DialogStateBase
    {
        public EasterEggState(IUserContext context) : base(context)
        {
        }

        public override void HandleUserRequest(AliceRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}