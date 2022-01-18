namespace AliceMafia.PlayerState
{
    public class NightResultState : PlayerStateBase
    {
        public NightResultState(Player context, GameContext gameContext) : base(context, gameContext)
        {
        }

        public override UserResponse HandleUserRequest(UserRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}