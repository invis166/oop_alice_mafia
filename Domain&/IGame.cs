﻿using System.Collections.Generic;

namespace AliceMafia
{
    public interface IGame
    {
        public List<IPlayer> Players{ get; }
        
        public void AddPlayer(IPlayer player);
    }
}