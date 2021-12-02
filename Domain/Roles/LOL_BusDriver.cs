using AliceMafia.Action;
using AliceMafia.Setting;

namespace AliceMafia
{
    public class LOL_BusDriver : RoleBase
    {
        public string Name => "Водитель автобуса";

        public (int, int) SwapRoles()
        {
            return (0, 1);
        }

        public LOL_BusDriver(GameState state, IRoleSetting setting) : base(state, setting)
        {
        }

        public override IRoleSetting Setting { get; protected set; }
        public override int Priority { get; }
        public override RoleActionBase NightAction { get; }
    }
}