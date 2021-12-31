using System.Collections.Generic;

namespace AliceMafia.Setting
{
    public interface IGameSetting
    {
        public string SettingName { get; }
        public IGeneralSetting GeneralMessages { get; }
        public Dictionary<string, IRoleSetting> roles { get; }
    }
}