namespace AliceMafia
{
    public class Civilian : RoleBase
    {
        public string Name => "Мирный житель";
        public void NightAction(IPlayer player)
        {
        }
    }
}