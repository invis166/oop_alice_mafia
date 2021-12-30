using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace AliceMafia.Voting
{
    public class Vote<TVal> : IVote<TVal>
    {
        private ConcurrentDictionary<TVal, int> voteCounter;
        public int totalVoteCounter;

        public Vote()
        {
            voteCounter = new ConcurrentDictionary<TVal, int>();
        }

        public List<TVal> GetResult()
        {
            var maxCount = voteCounter.Max(pair => pair.Value);

            return voteCounter
                .Where(pair => pair.Value == maxCount)
                .Select(pair => pair.Key)
                .ToList();
        }

        public void AddVote(TVal voteFor)
        {
            totalVoteCounter++;
            if (!voteCounter.ContainsKey(voteFor))
                voteCounter[voteFor] = 0;
            voteCounter[voteFor] += 1;
        }
    }
}