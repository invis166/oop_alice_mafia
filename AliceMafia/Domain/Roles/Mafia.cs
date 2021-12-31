using AliceMafia.Action;

namespace AliceMafia
{
    public class Mafia : RoleBase
    {
        public override int Priority => 1;
        public override RoleActionBase NightAction { get; protected set; }
        
        public Mafia(RoleActionBase action) : base(action)
        {
        }
    }
}