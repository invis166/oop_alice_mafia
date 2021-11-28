using System.Collections.Generic;

namespace AliceMafia.Voting
{
    public interface IVote<TVal>
    {
        public List<TVal> GetResult();
        public void AddVote(TVal voteFor);
    }
}