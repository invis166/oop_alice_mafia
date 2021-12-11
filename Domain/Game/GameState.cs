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
        public IPlayer HealedPlayer { get; set; }
        public IPlayer PlayerWithAlibi { get; set; }
        public IPlayer CheckedBySheriff { get; set; }
        public bool IsFirstDay;
        public HashSet<IPlayer> AlivePlayers { get; set; }
        public List<IPlayer> AboutToKillPlayers { get; set; }
        public TimeOfDay TimeOfDay { get; set; }
        public Vote<IPlayer> Voting { get; set; }
        public int WhoseTurn;

        public GameState()
        {
            WhoseTurn = 1;
            HealedPlayer = default;
            PlayerWithAlibi = default;
            CheckedBySheriff = default;
            IsFirstDay = true;
            AlivePlayers = new HashSet<IPlayer>();
            AboutToKillPlayers = new List<IPlayer>();
            TimeOfDay = TimeOfDay.Day;
            Voting = new Vote<IPlayer>();
        }

        public void Clear()
        {
            HealedPlayer = default;
            PlayerWithAlibi = default;
            AlivePlayers.ExceptWith(AboutToKillPlayers.ToHashSet());
            Voting = new Vote<IPlayer>();
            AboutToKillPlayers.Clear();
        }
    }

    
}