using System.Collections.Generic;
using System.Linq;

namespace AliceMafia.Voting
{
    public class Vote<TVal> : IVote<TVal>
    {
        private Dictionary<TVal, int> voteCounter;

        public Vote()
        {
            voteCounter = new Dictionary<TVal, int>();
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
            if (!voteCounter.ContainsKey(voteFor))
                voteCounter[voteFor] = 0;
            voteCounter[voteFor] += 1;
        }
    }
}