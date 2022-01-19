using System.Linq;

namespace AliceMafia.PlayerState
{
    public class NightWaitingState : PlayerStateBase // чек
    {
        public NightWaitingState(Player context, GameContext gameContext) : base(context, gameContext)
        {
        }

        public override UserResponse HandleUserRequest(UserRequest request)
        {
            var playerPriority = context.Role.Priority;

            if (!context.HasVoted && playerPriority == gameContext.State.WhoseTurn)
            {
                // наступил черед игрока, оповещаем его
                context.State = new NightActionState(context, gameContext);
                return new UserResponse
                {
                    Title = gameContext.Setting.roles[context.Role.GetType().Name].NightActionMessage,
                    Buttons = gameContext.State.AlivePlayers
                        .Where(x => x.Id != context.Id || x.Role.NightAction.CanActWithItself)
                        .ToDictionary(keySelector: player => player.Id, elementSelector: player => player.Name)
                };
            }
            
            return new UserResponse {Title = gameContext.Setting.GeneralMessages.NightWaitingMessage};
        }
    }
}