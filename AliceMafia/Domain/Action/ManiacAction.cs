namespace AliceMafia.Action
{
    public class ManiacAction : RoleActionBase
    {
        public ManiacAction(GameState gameState) : base(gameState)
        {
        }

        public override bool CanActWithItself => false;
        public override void DoAction(Player player) => gameState.AboutToKillPlayers.Add(player);
    }
}