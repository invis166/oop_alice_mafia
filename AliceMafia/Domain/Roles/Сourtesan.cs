using AliceMafia.Action;

namespace AliceMafia
{
    public class Courtesan : RoleBase
    {
        public override int Priority => 3;
        public override RoleActionBase NightAction { get; protected set; }
        
        public Courtesan(RoleActionBase action) : base(action)
        {
        }
    }
}