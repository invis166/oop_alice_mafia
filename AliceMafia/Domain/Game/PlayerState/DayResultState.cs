using System.Linq;

namespace AliceMafia.PlayerState
{
    public class DayResultState : PlayerStateBase
    {
        public DayResultState(Player context, GameContext gameContext) : base(context, gameContext)
        {
        }

        public override UserResponse HandleUserRequest(UserRequest request)
        {
            var currentPlayer = gameContext.State.GetAlivePlayerById(request.UserId);
            currentPlayer.State = new NightWaitingState(context, gameContext);
            var voteResult = gameContext.State.Voting.GetResult();
            if (gameContext.State.AlivePlayers.All(player => !(player.State is DayResultState)))
                // не какой нибудь
                gameContext.State.Clear();
            if (voteResult.Count == 1)
            {
                var jailedPlayer = voteResult.First();
                return new UserResponse { Title = gameContext.Setting.GeneralMessages.GetJailMessage(jailedPlayer.Name) };
            }

            return new UserResponse { Title = gameContext.Setting.GeneralMessages.UndecidedJailMessage };
        }
    }
}