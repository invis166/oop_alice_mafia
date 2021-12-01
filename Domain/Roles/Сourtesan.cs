using AliceMafia.Action;
using AliceMafia.Setting;

namespace AliceMafia
{
    public class Сourtesan : RoleBase
    {
        public override IRoleSetting Setting { get; protected set; }
        public override int Priority => 3;
        public override RoleActionBase NightAction { get; }
        
        public Сourtesan(GameState state, IRoleSetting setting) : base(state, setting)
        {
            NightAction = new CourtesanAction(state);
        }
    }
}