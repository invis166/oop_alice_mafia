namespace AliceMafia.Application
{
    public class JoinGameState : DialogStateBase
    {
        public JoinGameState(IUserContext context) : base(context)
        {
        }

        public override void HandleUserRequest(AliceRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}