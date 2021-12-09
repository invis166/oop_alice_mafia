namespace AliceMafia.Action
{
    public class MafiaAction : RoleActionBase
    {
        public MafiaAction(GameState gameState) : base(gameState)
        {
        }

        public override bool CanActWithItself { get; }
        public override void DoAction(IPlayer player)
        {
            gameState.Voting.AddVote(player);
        }
    }
}