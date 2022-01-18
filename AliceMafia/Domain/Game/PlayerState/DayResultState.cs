namespace AliceMafia.PlayerState
{
    public class DayResultState : PlayerStateBase
    {
        public DayResultState(Player context, GameContext gameContext) : base(context, gameContext)
        {
        }

        public override UserResponse HandleUserRequest(UserRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}