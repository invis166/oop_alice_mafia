using System;
using System.Collections.Generic;

namespace AliceMafia.Setting
{
    public interface IRoleSetting
    {
        public string Name { get; }
        public string NightActionMessage { get; }
    }
}