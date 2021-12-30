using System.Collections.Generic;

namespace AliceMafia.Setting
{
    public interface IGameSetting
    {
        public IGeneralSetting GeneralMessages { get; }
        public Dictionary<string, IRoleSetting> roles { get; }
    }
}