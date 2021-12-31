using AliceMafia.Action;

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