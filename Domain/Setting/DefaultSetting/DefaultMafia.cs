namespace AliceMafia.Setting.DefaultSetting
{
    public class DefaultMafia : IRoleSetting
    {
        public string Name => "Мафия";
        public string NightActionMessage { get; }
    }
}