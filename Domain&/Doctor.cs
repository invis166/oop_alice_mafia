namespace AliceMafia
{
    public class Doctor : IRole
    {
        public string Name => "Доктор";
        
        public void NightAction(IPlayer player)
        {
            // Game.Heal(IPlayer player);
        }
    }
}