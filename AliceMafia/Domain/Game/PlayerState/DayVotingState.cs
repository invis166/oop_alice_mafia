using System.Linq;

namespace AliceMafia.PlayerState
{
    public class DayVotingState : PlayerStateBase
    {
        public DayVotingState(Player context, GameContext gameContext) : base(context, gameContext)
        {
        }

        public override UserResponse HandleUserRequest(UserRequest request)
        {
            if (gameContext.State.DaysCounter == 0)
                return HandleFirstDay();
            
            return HandleVote(request.Payload);
        }
        
        private UserResponse HandleFirstDay()
        {
            context.State = new DayWaitingState(context, gameContext);
            var gameState = gameContext.State;
            string roleName;
            if (gameState.AlivePlayers.Count(player => player.State is DayWaitingState) == gameState.AlivePlayers.Count)
            {
                gameState.TimeOfDay = TimeOfDay.Night;
                gameState.DaysCounter++;
                roleName = gameContext.Setting.roles[context.Role.GetType().Name].Name;
                return new UserResponse {Title = $"Ваша роль {roleName}"};
            }

            roleName = gameContext.Setting.roles[context.Role.GetType().Name].Name;
            return new UserResponse {Title = $"Ваша роль {roleName}"};
        }
        
        private UserResponse HandleVote(string chosenPlayer)
        {
            gameContext.State.Voting.AddVote(Game.GetAlivePlayerById(gameContext.State, chosenPlayer));
            context.State = new DayWaitingState(context, gameContext);

            if (gameContext.State.Voting.totalVoteCounter == gameContext.State.AlivePlayers.Count)
            {
                gameContext.State.TimeOfDay = TimeOfDay.Night;
                gameContext.State.DaysCounter++;
                
                var votingResult = gameContext.State.Voting.GetResult();
                if (votingResult.Count == 1)
                    gameContext.State.AlivePlayers.Remove(votingResult.First());
                
                return new UserResponse {Title = gameContext.Setting.GeneralMessages.DayEndMessage};
            }

            return new UserResponse {Title = gameContext.Setting.GeneralMessages.DayWaitingMessage};
        }
    }
}