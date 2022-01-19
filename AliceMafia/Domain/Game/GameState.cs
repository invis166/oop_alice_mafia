using System.Collections.Generic;
using System.Linq;
using AliceMafia.Voting;

namespace AliceMafia
{
    public class GameState
    {
        public Player HealedPlayer { get; set; }
        public Player PlayerWithAlibi { get; set; }
        public Player CheckedBySheriff { get; set; }
        public HashSet<Player> AlivePlayers { get; set; }
        public List<Player> KilledAtNightPlayers { get; set; }
        public Vote<Player> Voting { get; set; }
        public List<Player> DayVotingResult { get; set; }
        public int WhoseTurn;

        public GameState()
        {
            WhoseTurn = 1;
            HealedPlayer = default;
            PlayerWithAlibi = default;
            CheckedBySheriff = default;
            AlivePlayers = new HashSet<Player>();
            KilledAtNightPlayers = new List<Player>();
            Voting = new Vote<Player>();
            DayVotingResult = new List<Player>();
        }

        public void Clear()
        {
            HealedPlayer = default;
            PlayerWithAlibi = default;
            CheckedBySheriff = default;
            Voting = new Vote<Player>();
            KilledAtNightPlayers.Clear();
        }
        
        public Player GetAlivePlayerById(string id) => AlivePlayers.First(player => player.Id == id);
        
        public int NextPriority(int priority) => AlivePlayers
            .Select(x => x.Role.Priority)
            .OrderBy(x => x)
            .FirstOrDefault(x => x > priority);
    }

    
}