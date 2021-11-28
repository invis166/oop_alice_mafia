namespace AliceMafia
{
    public class Civilian : IRole
    {
        public string Name => "Мирный житель";
        public void NightAction(IPlayer player)
        {
        }
    }
}