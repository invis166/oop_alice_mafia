using AliceMafia.Action;
using AliceMafia.Setting;

namespace AliceMafia
{
    public class Doctor : RoleBase
    {
        public override IRoleSetting Setting { get; protected set; }
        public override int Priority => 4;
        public override RoleActionBase NightAction { get; protected set; }
        
        public Doctor(RoleActionBase action, IRoleSetting setting) : base(action, setting)
        {
        }
    }
}