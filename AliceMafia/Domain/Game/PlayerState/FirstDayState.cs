using System.Linq;

namespace AliceMafia.PlayerState
{
    public class FirstDayState : PlayerStateBase // чек
    {
        public FirstDayState(Player context, GameContext gameContext) : base(context, gameContext)
        {
        }

        public override UserResponse HandleUserRequest(UserRequest request)
        {
            context.State = new DayWaitingState(context, gameContext);
            if (gameContext.State.AlivePlayers.Count(player => player.State is DayWaitingState) == gameContext.State.AlivePlayers.Count)
            {
                // день закончен, все узнали свои роли, можно перейти к ночи
                foreach (var player in gameContext.State.AlivePlayers)
                    player.State = new NightWaitingState(context, gameContext);
            }

            var roleName = gameContext.Setting.roles[context.Role.GetType().Name].Name;
            return new UserResponse {Title = $"Ваша роль {roleName}"};
        }
    }
}