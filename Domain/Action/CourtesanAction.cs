namespace AliceMafia.Action
{
    public class CourtesanAction : RoleActionBase
    {
        public CourtesanAction(GameState gameState) : base(gameState)
        {
        }

        public override bool CanActWithItself => true;
        public override void DoAction(IPlayer player) => gameState.PlayerWithAlibi = player;
    }
}