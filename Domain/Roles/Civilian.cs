using AliceMafia.Action;
using AliceMafia.Setting;

namespace AliceMafia
{
    public class Civilian : RoleBase
    {
        public override IRoleSetting Setting { get; protected set; }
        public override int Priority => -1;
        public override RoleActionBase NightAction { get; protected set; }

        public Civilian(RoleActionBase action, IRoleSetting setting) : base(action, setting)
        {
        }
    }
}