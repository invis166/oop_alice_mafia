using AliceMafia.Action;

namespace AliceMafia
{
    public class Doctor : RoleBase
    {
        public override int Priority => 4;
        public override RoleActionBase NightAction { get; protected set; }
        
        public Doctor(RoleActionBase action) : base(action)
        {
        }
    }
}