namespace AliceMafia.Action
{
    public class MafiaAction : RoleActionBase
    {
        public MafiaAction(GameState gameState) : base(gameState)
        {
        }

        public override bool CanActWithItself { get; }
        public override void DoAction(Player player)
        {
            gameState.Voting.AddVote(player);
        }
    }
}