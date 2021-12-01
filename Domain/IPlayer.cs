namespace AliceMafia
{
    public interface IPlayer
    {
        public string Id { get; }
        public string Name { get; }
        public RoleBase Role { get; }
    }
}