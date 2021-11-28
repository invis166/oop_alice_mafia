namespace AliceMafia.Setting
{
    public interface IGameSetting
    {
        public IMafiaSetting MafiaGame { get; set; }
        public IRoleSetting Civilian { get; set; }
        public IRoleSetting Mafia { get; set; }
        public IRoleSetting Doctor { get; set; }
        public IRoleSetting Sheriff { get; set; }
    }
}