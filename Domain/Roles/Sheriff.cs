using AliceMafia.Action;
using AliceMafia.Setting;

namespace AliceMafia
{
    public class Sheriff : RoleBase
    {
        public override int Priority => 5;
        public override RoleActionBase NightAction { get; protected set; }
        
        public Sheriff(RoleActionBase action) : base(action)
        {
        }
    }
}