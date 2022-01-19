namespace AliceMafia.PlayerState
{
    public class DeadState : PlayerStateBase
    {
        public DeadState(Player context, GameContext gameContext) : base(context, gameContext)
        {
        }

        public override UserResponse HandleUserRequest(UserRequest request)
        {
            return new UserResponse {Title = gameContext.Setting.GeneralMessages.DeathMessage};
        }
    }
}