using System;
using System.Collections.Generic;
using System.Security.Policy;

namespace AliceMafia
{
    public class GameState
    {
        public IPlayer HealedPlayer { get; set; }
        public IPlayer PlayerWithAlibi { get; set; }
        public IPlayer CheckedBySheriff { get; set; }
        public int GameCycleCount { get; set; }
        public HashSet<IPlayer> AlivePlayers { get; set; }
        public List<IPlayer> AboutToKillPlayers { get; set; }

        public void Clear()
        {
            HealedPlayer = default;
            PlayerWithAlibi = default;
            AboutToKillPlayers.Clear();
        }
    }

    
}