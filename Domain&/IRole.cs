namespace AliceMafia
{
    public interface IRole
    {
        //i'm not sure вообще рефлексией по названиям классов можно наверное
        public string Name { get; }

        public int/*ну инт типа номер игрока*/ VoteAfterNight()
        {
            return 42;
        }
    }
}