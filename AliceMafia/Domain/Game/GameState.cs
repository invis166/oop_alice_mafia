using System.Collections.Generic;
using AliceMafia.Voting;

namespace AliceMafia
{
    public enum TimeOfDay
    {
        Day,
        Night,
    }
    
    public class GameState
    {
        public Player HealedPlayer { get; set; }
        public Player PlayerWithAlibi { get; set; }
        public Player CheckedBySheriff { get; set; }
        public int DaysCounter;
        public HashSet<Player> AlivePlayers { get; set; }
        public List<Player> KilledAtNightPlayers { get; set; }
        public TimeOfDay TimeOfDay { get; set; }
        public Vote<Player> Voting { get; set; }
        public List<Player> DayVotingResult { get; set; }
        public int WhoseTurn;

        public GameState()
        {
            WhoseTurn = 1;
            HealedPlayer = default;
            PlayerWithAlibi = default;
            CheckedBySheriff = default;
            DaysCounter = 0;
            AlivePlayers = new HashSet<Player>();
            KilledAtNightPlayers = new List<Player>();
            TimeOfDay = TimeOfDay.Day;
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
    }

    
}