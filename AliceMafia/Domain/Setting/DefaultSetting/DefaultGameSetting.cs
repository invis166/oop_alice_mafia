using System.Collections.Generic;

namespace AliceMafia.Setting.DefaultSetting
{
    public class DefaultGameSetting : IGameSetting
    {
        public string SettingName => "Стандартный сеттинг";
        public IGeneralSetting GeneralMessages { get; }
        public Dictionary<string, IRoleSetting> roles { get; }

        public DefaultGameSetting()
        {
            GeneralMessages = new DefaultGeneral();
            roles = new Dictionary<string, IRoleSetting>
            {
                ["Civilian"] = new DefaultCivilian(),
                ["Doctor"] = new DefaultDoctor(),
                ["Mafia"] = new DefaultMafia(),
                ["Sheriff"] = new DefaultSheriff(),
                ["Courtesan"] = new DefaultCourtesan()
            };
        }
    }
}