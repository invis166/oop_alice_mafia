namespace AliceMafia
{
    public class Doctor : IRole
    {
        public string Name => "Доктор";

        public int HealPlayer()
        {
            return 42;
        }
    }
}