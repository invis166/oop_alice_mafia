using AliceMafia.Action;
using AliceMafia.Setting;

namespace AliceMafia
{
    public class Sheriff : RoleBase
    {
        public override IRoleSetting Setting { get; protected set; }
        public override int Priority => 5;
        public override RoleActionBase NightAction { get; protected set; }
        
        public Sheriff(RoleActionBase action, IRoleSetting setting) : base(action, setting)
        {
        }
    }
}