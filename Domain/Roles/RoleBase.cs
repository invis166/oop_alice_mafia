using AliceMafia.Action;
using AliceMafia.Setting;

namespace AliceMafia
{
    public abstract class RoleBase
    {
        private GameState gameState;
        public readonly IRoleSetting Setting;
        public abstract int Priority { get; }
        public abstract RoleActionBase NightAction { get; }
        
        public RoleBase(GameState state, IRoleSetting setting)
        {
            gameState = state;
            Setting = setting;
        }
        
    }
}