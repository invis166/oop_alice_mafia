namespace AliceMafia
{
    public interface IRole
    {
        public string Name { get; }

        public void NightAction(IPlayer player);
    }
}