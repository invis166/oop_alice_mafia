namespace AliceMafia
{
    public class Mafia : IRole
    {
        public string Name => "Мафия";

        public int /*ну инт типа номер игрока*/ VoteForKill()
        {
            return 42;
        }
    }
}