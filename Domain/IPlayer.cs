namespace AliceMafia
{
    public interface IPlayer
    {
        public string PlayerId { get; }
        public string Name { get; }
        public RoleBase Role { get; }
    }
}