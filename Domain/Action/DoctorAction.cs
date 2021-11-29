namespace AliceMafia.Action
{
    public class DoctorAction : RoleActionBase
    {
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