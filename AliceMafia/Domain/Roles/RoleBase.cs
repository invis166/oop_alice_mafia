using AliceMafia.Action;

namespace AliceMafia
{
    public abstract class RoleBase
    {
        public abstract int Priority { get; }
        public abstract RoleActionBase NightAction { get; protected set; }

        protected RoleBase(RoleActionBase action)
        {
            NightAction = action;
        }
    }
}