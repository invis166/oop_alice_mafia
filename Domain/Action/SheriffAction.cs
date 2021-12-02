namespace AliceMafia.Action
{
    public class SheriffAction : RoleActionBase
    {
        public SheriffAction(GameState gameState) : base(gameState)
        {
        }

        public override bool CanActWithItself => false;
        public override void DoAction(IPlayer player) => gameState.CheckedBySheriff = player;
    }
}