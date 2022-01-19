namespace AliceMafia.Setting.DefaultSetting
{
    public class DefaultCivilian : IRoleSetting
    {
        public string Name => "Мирный житель";
        public string NightActionMessage => "К сожалению, вы не можете ничего делать, так что вы просто мирно спите и надеетесь на лучшее.";
    }
}