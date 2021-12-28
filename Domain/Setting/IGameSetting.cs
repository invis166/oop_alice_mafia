using System.Collections.Generic;

namespace AliceMafia.Setting
{
    public interface IGameSetting
    {
        public IGeneralSetting GeneralMessages { get; }
        //[   ]
        // public IRoleSetting Civilian { get; }
        // public IRoleSetting Mafia { get; }
        // public IRoleSetting Doctor { get; }
        // public IRoleSetting Sheriff { get; }
        // public IRoleSetting Courtesan { get; }
        public Dictionary<string, IRoleSetting> roles { get; }
    }
}