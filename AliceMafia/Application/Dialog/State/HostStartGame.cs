namespace AliceMafia.Application
{
    public class HostStartGame : DialogStateBase
    {
        public HostStartGame(IUserContext context) : base(context)
        {
        }

        public override void HandleUserRequest(AliceRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}