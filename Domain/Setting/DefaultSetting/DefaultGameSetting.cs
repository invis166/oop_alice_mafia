using System.Reflection;

namespace AliceMafia.Setting.DefaultSetting
{
    public class DefaultGameSetting : IGameSetting
    {
        public IGeneralSetting GeneralMessages { get; }
        public IRoleSetting Civilian { get; }
        public IRoleSetting Mafia { get; }
        public IRoleSetting Doctor { get; }
        public IRoleSetting Sheriff { get; }
        
        public DefaultGameSetting()
        {
            GeneralMessages = new DefaultGeneral();
            Civilian = new DefaultCivilian();
            Mafia = new DefaultMafia();
            Doctor = new DefaultDoctor();
            Sheriff = new DefaultMafia();
        }
    }
}