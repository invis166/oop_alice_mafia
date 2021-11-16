namespace AliceMafia
{
    public class LOL_BusDriver : IRole
    {
        public string Name => "Водитель автобуса";

        public (int, int) SwapRoles()
        {
            return (0, 1);
        }
    }
}