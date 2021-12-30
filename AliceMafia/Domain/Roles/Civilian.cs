using AliceMafia.Action;
using AliceMafia.Setting;

namespace AliceMafia
{
    public class Civilian : RoleBase
    {
        public override int Priority => -1;
        public override RoleActionBase NightAction { get; protected set; }

        public Civilian(RoleActionBase action) : base(action)
        {
        }
    }
}