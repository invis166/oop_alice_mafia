using System.Linq;
using AliceMafia.Voting;


namespace AliceMafia.PlayerState
{
    public class NightActionState : PlayerStateBase
    {
        public NightActionState(Player context, GameContext gameContext) : base(context, gameContext)
        {
        }

        public override UserResponse HandleUserRequest(UserRequest request)
        {
            context.Role.NightAction.DoAction(gameContext.State.GetAlivePlayerById(request.Payload));
            context.State = new NightWaitingState(context, gameContext);
            context.HasVoted = true; // надо избавиться

            var playerPriority = context.Role.Priority;
            var countOfNotVotedPlayers = gameContext.State.AlivePlayers.Count(
                x => x.Role.Priority == gameContext.State.WhoseTurn && !x.HasVoted);
            if (countOfNotVotedPlayers == 0 && !(context.Role is Civilian))
            {
                var nextPriority = gameContext.State.NextPriority(playerPriority);
                if (nextPriority == 0)
                {
                    HandleNightEnd();
                    nextPriority = 1;
                }

                gameContext.State.WhoseTurn = nextPriority;
            }

            if (context.Role is Sheriff)
            {
                var isMafia = gameContext.State.CheckedBySheriff.Role is Mafia;
                var mafiaName = gameContext.Setting.roles["Mafia"].Name;
                return new UserResponse {Title = "Игрок " + (isMafia ? "" : "не ") + mafiaName};
            }

            return new UserResponse {Title = gameContext.Setting.GeneralMessages.AfterVotingMessage};
        }

        private void HandleNightEnd()
        {
            // следующим шагом объявляем результаты
            foreach (var player in gameContext.State.AlivePlayers)
            {
                player.HasVoted = false; // исправить надо!!!
                player.State = new NightResultState(player, gameContext);
            }
            
            var mafiaVoteResult = gameContext.State.Voting.GetResult();
            if (mafiaVoteResult.Count == 1)
                gameContext.State.KilledAtNightPlayers.Add(mafiaVoteResult.First());
            
            gameContext.State.Voting = new Vote<Player>();
            if (gameContext.State.HealedPlayer != null)
                gameContext.State.KilledAtNightPlayers.Remove(gameContext.State.HealedPlayer);
            
            gameContext.State.AlivePlayers.ExceptWith(gameContext.State.KilledAtNightPlayers);
            foreach (var player in gameContext.State.KilledAtNightPlayers)
                player.State = new DeadState(player, gameContext);
        }

    }
}