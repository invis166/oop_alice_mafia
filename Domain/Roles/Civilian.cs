using AliceMafia.Action;
using AliceMafia.Setting;

namespace AliceMafia
{
    public class Civilian : RoleBase
    {
        public override IRoleSetting Setting { get; protected set; }
        public override int Priority => -1;
        public override RoleActionBase NightAction { get; }

        public Civilian(GameState state, IRoleSetting setting) : base(state, setting)
        {
        }
    }
}