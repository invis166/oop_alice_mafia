namespace AliceMafia.Action
{
    public class DoctorAction : RoleActionBase
    {
        public override bool CanActWithItself => true;
        public DoctorAction(GameState gameState) : base(gameState)
        {
        }

        public override void DoAction(IPlayer player)
        {
            gameState.HealedPlayer = player;
            gameState.AlivePlayers.Add(player);
        }
    }
}