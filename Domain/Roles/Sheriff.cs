using AliceMafia.Action;
using AliceMafia.Setting;

namespace AliceMafia
{
    public class Sheriff : RoleBase
    {
        public override IRoleSetting Setting { get; protected set; }
        public override int Priority => 5;
        public override RoleActionBase NightAction { get; }
        
        public Sheriff(GameState state, IRoleSetting setting) : base(state, setting)
        {
            NightAction = new SheriffAction(state);
        }
    }
}