namespace AliceMafia.PlayerState
{
    public class NightWaitingState : PlayerStateBase
    {
        public NightWaitingState(Player context, GameContext gameContext) : base(context, gameContext)
        {
        }

        public override UserResponse HandleUserRequest(UserRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}