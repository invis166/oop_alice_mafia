namespace AliceMafia
{
    public interface IPlayer
    {
        public string PlayerId { get; }
        public RoleBase Role { get; }
    }
}