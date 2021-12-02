using AliceMafia.Action;
using AliceMafia.Setting;

namespace AliceMafia
{
    public class Doctor : RoleBase
    {
        public override IRoleSetting Setting { get; protected set; }
        public override int Priority => 4;
        public override RoleActionBase NightAction { get; }
        
        public Doctor(GameState state, IRoleSetting setting) : base(state, setting)
        {
            NightAction = new DoctorAction(state);
        }
    }
}