namespace AliceMafia.PlayerState
{
    public abstract class PlayerStateBase
    {
        protected Player context;
        protected GameContext gameContext;
        
        public PlayerStateBase(Player context, GameContext gameContext)
        {
            this.context = context;
            this.gameContext = gameContext;
        }

        public abstract UserResponse HandleUserRequest(UserRequest request);
    }
}