namespace AliceMafia.PlayerState
{
    public class NightActionState : PlayerStateBase
    {
        public NightActionState(Player context, GameContext gameContext) : base(context, gameContext)
        {
        }

        public override UserResponse HandleUserRequest(UserRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}