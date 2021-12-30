using System.Collections.Generic;
using System.Linq;
using AliceMafia.Voting;

namespace AliceMafia
{
    public enum TimeOfDay
    {
        Day,
        Night
    }
    
    public class GameState
    {
        public Player HealedPlayer { get; set; }
        public Player PlayerWithAlibi { get; set; }
        public Player CheckedBySheriff { get; set; }
        public bool IsFirstDay;
        public HashSet<Player> AlivePlayers { get; set; }
        public List<Player> AboutToKillPlayers { get; set; }
        public TimeOfDay TimeOfDay { get; set; }
        public Vote<Player> Voting { get; set; }
        public int WhoseTurn;

        public GameState()
        {
            WhoseTurn = 1;
            HealedPlayer = default;
            PlayerWithAlibi = default;
            CheckedBySheriff = default;
            IsFirstDay = true;
            AlivePlayers = new HashSet<Player>();
            AboutToKillPlayers = new List<Player>();
            TimeOfDay = TimeOfDay.Day;
            Voting = new Vote<Player>();
        }

        public void Clear()
        {
            HealedPlayer = default;
            PlayerWithAlibi = default;
            Voting = new Vote<Player>();
            AboutToKillPlayers.Clear();
        }
    }

    
}