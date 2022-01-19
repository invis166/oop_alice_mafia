using System.Linq;

namespace AliceMafia.PlayerState
{
    public class NightResultState : PlayerStateBase // чек
    {
        public NightResultState(Player context, GameContext gameContext) : base(context, gameContext)
        {
        }

        public override UserResponse HandleUserRequest(UserRequest request)
        {
            context.State = new DayVotingState(context, gameContext);
            var killMessage = gameContext.Setting.GeneralMessages.GetKillMessage(gameContext.State
                .KilledAtNightPlayers
                .Where(x => x.Id != gameContext.State.HealedPlayer?.Id)
                .Select(x => x.Name).ToList());
            
            return new UserResponse
            {
                Title = $"{killMessage}. {gameContext.Setting.GeneralMessages.DayVotingMessage}",
                Buttons = gameContext.State.AlivePlayers
                    .Where(x => x.Id != context.Id && x.Id != gameContext.State.PlayerWithAlibi?.Id)
                    .ToDictionary(keySelector: player => player.Id, elementSelector: player => player.Name)
            };
        }
    }
}