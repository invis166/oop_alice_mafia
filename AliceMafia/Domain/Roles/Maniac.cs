﻿using AliceMafia.Action;
using AliceMafia.Setting;

namespace AliceMafia
{
    public class Maniac : RoleBase
    {
        public override int Priority => 2;
        public override RoleActionBase NightAction { get; protected set; }
        
        public Maniac(RoleActionBase action) : base(action)
        {
        }
    }
}