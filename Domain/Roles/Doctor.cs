using AliceMafia.Action;

namespace AliceMafia
{
    public class Doctor : RoleBase
    {
        public override int Priority => 5;
        public override RoleActionBase NightAction { get; }
        
        public Doctor(GameState state) : base(state)
        {
            NightAction = new DoctorAction(state);
        }

    }
}