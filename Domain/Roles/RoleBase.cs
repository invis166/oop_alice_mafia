using AliceMafia.Action;
using AliceMafia.Setting;

namespace AliceMafia
{
    public abstract class RoleBase
    {
        private GameState gameState;
        public abstract IRoleSetting Setting { get; protected set; }
        public abstract int Priority { get; }
        public abstract RoleActionBase NightAction { get; }

        protected RoleBase(GameState state, IRoleSetting setting)
        {
            gameState = state;
            Setting = setting;
        }
        
    }
}