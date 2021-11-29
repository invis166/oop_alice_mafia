using AliceMafia.Action;

namespace AliceMafia
{
    public abstract class RoleBase
    {
        private GameState gameState;
        public abstract int Priority { get; }
        public abstract RoleActionBase NightAction { get; }
        
        public RoleBase(GameState state)
        {
            gameState = state;
        }
        
    }
}