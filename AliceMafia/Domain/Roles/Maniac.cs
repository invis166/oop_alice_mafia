using AliceMafia.Action;

namespace AliceMafia
{
    public class Maniac : RoleBase
    {
        public override int Priority => 2;
        public override RoleActionBase NightAction { get; protected set; }
        
        public Maniac(RoleActionBase action) : base(action)
        {
        }
    }
}