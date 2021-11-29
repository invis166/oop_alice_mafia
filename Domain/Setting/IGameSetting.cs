namespace AliceMafia.Setting
{
    public interface IGameSetting
    {
        public IGeneralSetting GeneralMessages { get; }
        public IRoleSetting Civilian { get; }
        public IRoleSetting Mafia { get; }
        public IRoleSetting Doctor { get; }
        public IRoleSetting Sheriff { get; }
    }
}