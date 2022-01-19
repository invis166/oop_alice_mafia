namespace AliceMafia.PlayerState
{
    public class DayWaitingState : PlayerStateBase // чек
    {
        public DayWaitingState(Player context, GameContext gameContext) : base(context, gameContext)
        {
        }

        public override UserResponse HandleUserRequest(UserRequest request)
        {
            return new UserResponse {Title = gameContext.Setting.GeneralMessages.DayWaitingMessage};
        }
    }
}