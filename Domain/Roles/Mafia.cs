﻿using AliceMafia.Action;

namespace AliceMafia
{
    public class Mafia : RoleBase
    {
        public Mafia(GameState state) : base(state)
        {
        }

        public override int Priority { get; }
        public override RoleActionBase NightAction { get; }
    }
}