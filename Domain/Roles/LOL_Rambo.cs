using AliceMafia.Action;
using AliceMafia.Setting;

namespace AliceMafia
{
    public class LOL_Rambo : RoleBase
    {
        //вы не поверите, короче он гранату кидает и убивает челов с обеих сторон
        public string Name => "Рэмбо";

        public int ThrowGrenade()
        {
            return 1337;
        }

        public LOL_Rambo(RoleActionBase action, IRoleSetting setting) : base(action, setting)
        {
        }

        public override IRoleSetting Setting { get; protected set; }
        public override int Priority { get; }
        public override RoleActionBase NightAction { get; protected set; }
    }
}