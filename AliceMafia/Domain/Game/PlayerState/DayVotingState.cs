using System.Linq;

namespace AliceMafia.PlayerState
{
    public class DayVotingState : PlayerStateBase // чек
    {
        public DayVotingState(Player context, GameContext gameContext) : base(context, gameContext)
        {
        }

        public override UserResponse HandleUserRequest(UserRequest request)
        {
            gameContext.State.Voting.AddVote(gameContext.State.GetAlivePlayerById(request.Payload));
            context.State = new DayWaitingState(context, gameContext);

            if (gameContext.State.Voting.totalVoteCounter == gameContext.State.AlivePlayers.Count)
                HandleDayEnd();

            return new UserResponse {Title = gameContext.Setting.GeneralMessages.DayWaitingMessage};
        }

        private void HandleDayEnd()
        {
            // все сделали свой голос, переходим к объявлению результатов
            foreach (var player in gameContext.State.AlivePlayers)
                player.State = new DayResultState(player, gameContext);

            var votingResult = gameContext.State.Voting.GetResult();
            if (votingResult.Count == 1)
                gameContext.State.AlivePlayers.Remove(votingResult.First());
        }

    }
}