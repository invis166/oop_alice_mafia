namespace AliceMafia.Voting
{
    public interface IVoteHandler
    {
        public IPlayer[] GetResult();
        public void Vote(IPlayer whoVoted, IPlayer forWhomVoted);
    }
}