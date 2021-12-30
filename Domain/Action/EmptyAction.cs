namespace AliceMafia.Action
{
    public class EmptyAction : RoleActionBase
    {
        public EmptyAction(GameState gameState) : base(gameState)
        {
        }

        public override bool CanActWithItself => false;
        public override void DoAction(Player player)
        {
        }
    }
}