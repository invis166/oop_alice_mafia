using AliceMafia.Action;
using AliceMafia.Setting;

namespace AliceMafia
{
    public class Maniac : RoleBase
    {
        public override IRoleSetting Setting { get; protected set; }
        public override int Priority => 2;
        public override RoleActionBase NightAction { get; }
        
        public Maniac(GameState state, IRoleSetting setting) : base(state, setting)
        {
            NightAction = new ManiacAction(state);
        }
    }
}