namespace AliceMafia.Action
{
    public class DoctorAction : RoleActionBase
    {
        public DoctorAction(GameState gameState) : base(gameState)
        {
        }
        
        public override bool CanActWithItself => true;
        public override void DoAction(Player player) => gameState.HealedPlayer = player;
    }
}