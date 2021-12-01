using AliceMafia.Action;
using AliceMafia.Setting;

namespace AliceMafia
{
    public class Mafia : RoleBase
    {
        public override IRoleSetting Setting { get; protected set; }
        public override int Priority => 1;
        public override RoleActionBase NightAction { get; }
        
        public Mafia(GameState state, IRoleSetting setting) : base(state, setting)
        {
        }
    }
}