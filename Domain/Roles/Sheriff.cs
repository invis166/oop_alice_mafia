using AliceMafia.Action;

namespace AliceMafia
{
    public class Sheriff : RoleBase
    {
        public Sheriff(GameState state) : base(state)
        {
        }

        public override int Priority { get; }
        public override RoleActionBase NightAction { get; }
    }
}