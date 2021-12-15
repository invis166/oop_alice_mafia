using AliceMafia.Action;
using AliceMafia.Setting;

namespace AliceMafia
{
    public class Mafia : RoleBase
    {
        public override IRoleSetting Setting { get; protected set; }
        public override int Priority => 1;
        public override RoleActionBase NightAction { get; protected set; }
        
        public Mafia(RoleActionBase action, IRoleSetting setting) : base(action, setting)
        {
        }
    }
}