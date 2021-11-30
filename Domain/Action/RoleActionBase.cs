namespace AliceMafia.Action
{
    public abstract class RoleActionBase
    {
        protected GameState gameState;
        public abstract bool CanActWithItself { get; }
        
        public RoleActionBase(GameState gameState)
        {
            this.gameState = gameState;
        }
        
        public abstract void DoAction(IPlayer player);
    }
}