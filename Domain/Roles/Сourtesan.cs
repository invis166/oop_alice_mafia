using AliceMafia.Action;
using AliceMafia.Setting;

namespace AliceMafia
{
    public class Сourtesan : RoleBase
    {
        public override IRoleSetting Setting { get; protected set; }
        public override int Priority => 3;
        public override RoleActionBase NightAction { get; protected set; }
        
        public Сourtesan(RoleActionBase action, IRoleSetting setting) : base(action, setting)
        {
        }
    }
}