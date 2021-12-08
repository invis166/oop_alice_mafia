using System.Collections.Generic;
using System.Linq;

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
        public int GameCycleCount { get; set; }
        public HashSet<IPlayer> AlivePlayers { get; set; }
        public List<IPlayer> AboutToKillPlayers { get; set; }
        public TimeOfDay TimeOfDay { get; set; }
        public int VoteCounter = 0;
        public int WhoseTurn = 1;

        public void Clear()
        {
            HealedPlayer = default;
            PlayerWithAlibi = default;
            AlivePlayers.ExceptWith(AboutToKillPlayers.ToHashSet());
            AboutToKillPlayers.Clear();
        }
    }

    
}