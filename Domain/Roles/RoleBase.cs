using AliceMafia.Action;
using AliceMafia.Setting;

namespace AliceMafia
{
    public abstract class RoleBase
    {
        public abstract IRoleSetting Setting { get; protected set; }
        public abstract int Priority { get; }
        public abstract RoleActionBase NightAction { get; }

        protected RoleBase(IRoleSetting setting) => Setting = setting;
    }
}