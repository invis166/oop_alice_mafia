namespace AliceMafia
{
    public interface IRole
    {
        //i'm not sure вообще рефлексией по названиям классов можно наверное
        public string Name { get; }

        public void NightAction(IPlayer player);
    }
}